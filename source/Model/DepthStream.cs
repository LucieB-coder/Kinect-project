using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    using Microsoft.Kinect;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Media.Imaging;
    using System.Windows.Media;

    namespace Model
    {
        public class DepthStream : KinectStream
        {
            public DepthStream(KinectManager kinectManager) : base(kinectManager)
            {
            }

            private DepthFrameReader? DepthFrameReader { get; set; }

            public override void Start()
            {
                DepthFrameReader = KinectSensor.DepthFrameSource.OpenReader();
                this.DepthFrameReader = KinectSensor.DepthFrameSource.OpenReader();

                // wire handler for frame arrival
                DepthFrameReader.FrameArrived += FrameArrived;

                // create the depthFrameDescription from the depthFrameSource using rgba format
                FrameDescription depthFrameDescription = KinectSensor.DepthFrameSource.FrameDescription;

                // create the bitmap to display
                this.Bitmap = new WriteableBitmap(depthFrameDescription.Width, depthFrameDescription.Height, 96, 96, PixelFormats.Gray16, null);

            }

            private void FrameArrived(object sender, DepthFrameArrivedEventArgs e)
            {
                using (DepthFrame depthFrame = e.FrameReference.AcquireFrame())
                {
                    // If the depth frame is null, do nothing
                    if (depthFrame != null && Bitmap != null)
                    {
                        // Frame description to retreive the size of the frame
                        FrameDescription depthFrameDescription = depthFrame.FrameDescription;

                        // LockRawImageBuffer is used to retrieve the data of the new frame
                        using (KinectBuffer depthBuffer = depthFrame.LockImageBuffer())
                        {
                            // We lock the bitmap to be able to change its content
                            this.Bitmap.Lock();
                            // We check that our frame has the same size as our bitmap
                            if ((depthFrameDescription.Width == this.Bitmap.PixelWidth) && (depthFrameDescription.Height == this.Bitmap.PixelHeight))
                            {
                                // We send the new frame to the buffer of our bitmap 
                                uint bufferSize = (uint)(depthFrameDescription.Width * depthFrameDescription.Height * sizeof(ushort));
                                depthFrame.CopyFrameDataToIntPtr(
                                    this.Bitmap.BackBuffer,
                                    bufferSize);

                                // The new Invalidate method, specify the rectangle that must be changed in the bitmap (X and Y for the position, Widht and height, well, for widht and height
                                // In our case, we want to invalidate all the bitmap
                                this.Bitmap.AddDirtyRect(new Int32Rect
                                {
                                    X = 0,
                                    Y = 0,
                                    Width = this.Bitmap.PixelWidth,
                                    Height = this.Bitmap.PixelHeight
                                });
                            }
                            // We unlock the bitmap to unable the display
                            this.Bitmap.Unlock();
                            OnPropertyChanged(nameof(Bitmap));
                        }
                    }
                }
            }

            public override void Stop()
            {
                if (DepthFrameReader != null)
                {
                    DepthFrameReader.Dispose();
                }
            }
        }
    }

}

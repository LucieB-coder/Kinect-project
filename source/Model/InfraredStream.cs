using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;

namespace Model
{
    public class InfraredStream : KinectStream
    {
        public InfraredStream(KinectManager kinectManager) : base(kinectManager)
        {
        }

        private InfraredFrameReader? InfraredFrameReader { get; set; }

        public override void Start()
        {
            InfraredFrameReader = KinectSensor.InfraredFrameSource.OpenReader();

            // wire handler for frame arrival
            InfraredFrameReader.FrameArrived += FrameArrived;

            // create the colorFrameDescription from the ColorFrameSource using rgba format
            FrameDescription infraredFrameDescription = KinectSensor.InfraredFrameSource.FrameDescription;

            // create the bitmap to display
            this.Bitmap = new WriteableBitmap(infraredFrameDescription.Width, infraredFrameDescription.Height, 96, 96, PixelFormats.Gray16, null);

        }

        private void FrameArrived(object sender, InfraredFrameArrivedEventArgs e)
        {
            using (InfraredFrame infraredFrame = e.FrameReference.AcquireFrame())
            {
                // If the color frame is null, do nothing
                if (infraredFrame != null && Bitmap != null)
                {
                    // Frame description to retreive the size of the frame
                    FrameDescription infraredFrameDescription = infraredFrame.FrameDescription;

                    // LockRawImageBuffer is used to retrieve the data of the new frame
                    using (KinectBuffer colorBuffer = infraredFrame.LockImageBuffer())
                    {
                        // We lock the bitmap to be able to change its content
                        this.Bitmap.Lock();
                        // We check that our frame has the same size as our bitmap
                        if ((infraredFrameDescription.Width == this.Bitmap.PixelWidth) && (infraredFrameDescription.Height == this.Bitmap.PixelHeight))
                        {
                            // We send the new frame to the buffer of our bitmap 
                            uint bufferSize = (uint)(infraredFrameDescription.Width * infraredFrameDescription.Height * sizeof(ushort));
                            infraredFrame.CopyFrameDataToIntPtr(
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
            if (InfraredFrameReader != null)
            {
                InfraredFrameReader.Dispose();
            }
        }
    }
}

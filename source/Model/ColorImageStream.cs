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
    public class ColorImageStream : KinectStream
    {
        public ColorImageStream(KinectManager kinectManager) : base(kinectManager)
        {
        }

        public ColorFrame? ColorFrame { get; set; }
        public FrameDescription? FrameDescription { get; set; }
        private ColorFrameReader? ColorFrameReader { get; set; }

        public override void Start()
        {
            ColorFrameReader = KinectSensor.ColorFrameSource.OpenReader();
            this.ColorFrameReader = KinectSensor.ColorFrameSource.OpenReader();

            // wire handler for frame arrival
            ColorFrameReader.FrameArrived += FrameArrived;

            // create the colorFrameDescription from the ColorFrameSource using rgba format
            FrameDescription colorFrameDescription = KinectSensor.ColorFrameSource.CreateFrameDescription(ColorImageFormat.Rgba);

            // create the bitmap to display
            this.Bitmap = new WriteableBitmap(colorFrameDescription.Width, colorFrameDescription.Height, 96, 96, PixelFormats.Bgra32, null);

        }

        private void FrameArrived(object sender, ColorFrameArrivedEventArgs e)
        {
            using (ColorFrame colorFrame = e.FrameReference.AcquireFrame())
            {
                // If the color frame is null, do nothing
                if (colorFrame != null && Bitmap != null)
                {
                    // Frame description to retreive the size of the frame
                    FrameDescription colorFrameDescription = colorFrame.FrameDescription;

                    // LockRawImageBuffer is used to retrieve the data of the new frame
                    using (KinectBuffer colorBuffer = colorFrame.LockRawImageBuffer())
                    {
                        // We lock the bitmap to be able to change its content
                        this.Bitmap.Lock();
                        // We check that our frame has the same size as our bitmap
                        if ((colorFrameDescription.Width == this.Bitmap.PixelWidth) && (colorFrameDescription.Height == this.Bitmap.PixelHeight))
                        {
                            // We send the new frame to the buffer of our bitmap 
                            colorFrame.CopyConvertedFrameDataToIntPtr(
                                this.Bitmap.BackBuffer,
                                (uint)(colorFrameDescription.Width * colorFrameDescription.Height * 4),
                                ColorImageFormat.Bgra);

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
            ColorFrame.Dispose();
            KinectSensor.Close();
        }
    }
}

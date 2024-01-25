using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using System.Drawing;

namespace Model
{
    public class BodyColorStream : KinectStream
    {
        private BodyStream BodyStream;

        private ColorImageStream ColorImageStream;

        public BodyColorStream(KinectManager kinectManager) : base(kinectManager)
        {
            BodyStream = new BodyStream(kinectManager);
            ColorImageStream = new ColorImageStream(kinectManager);
        }

        public override void Start()
        {
            ColorImageStream.Start();
            BodyStream.Start();

            ColorImageStream.PropertyChanged += Stream_PropertyChanged;
            BodyStream.PropertyChanged += Stream_PropertyChanged;


            FrameDescription colorFrameDescription = KinectSensor.ColorFrameSource.CreateFrameDescription(ColorImageFormat.Rgba);

            // create the bitmap to display
            Bitmap = new WriteableBitmap(colorFrameDescription.Width, colorFrameDescription.Height, 96, 96, PixelFormats.Bgra32, null);

        }

        private void Stream_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (Bitmap != null && BodyStream.Bitmap != null && ColorImageStream.Bitmap != null) 
            {
                // Suppose bitmap1 and bitmap2 are your two BitmapSource objects
                WriteableBitmap bodyBitmap = BodyStream.Bitmap; // Initialize your first bitmap
                WriteableBitmap colorBitmap = ColorImageStream.Bitmap; // Initialize your second bitmap


                // Ensure that the dimensions of both bitmaps are the same
                if (bodyBitmap.PixelWidth == colorBitmap.PixelWidth && bodyBitmap.PixelHeight == colorBitmap.PixelHeight)
                {

                    // Lock all three bitmaps
                    bodyBitmap.Lock();
                    colorBitmap.Lock();
                    Bitmap.Lock();
                    // Copy the pixels from bitmap1 to the combinedBitmap
                    Bitmap.WritePixels(new Int32Rect(0, 0, bodyBitmap.PixelWidth, bodyBitmap.PixelHeight),
                                                bodyBitmap.BackBuffer,
                                                bodyBitmap.BackBufferStride,
                                                0);

                    // Calculate the offset for copying bitmap2
                    int offset = bodyBitmap.BackBufferStride * bodyBitmap.PixelHeight;

                    // Copy the pixels from bitmap2 to the combinedBitmap, using the calculated offset
                    Bitmap.WritePixels(new Int32Rect(0, 0, colorBitmap.PixelWidth, colorBitmap.PixelHeight),
                                                colorBitmap.BackBuffer,
                                                colorBitmap.BackBufferStride,
                                                offset);

                    // Ensure the changes are applied to the combinedBitmap
                    Bitmap.AddDirtyRect(new Int32Rect(0, 0, Bitmap.PixelWidth, Bitmap.PixelHeight));
                    // Unlock all three bitmaps
                    bodyBitmap.Unlock();
                    colorBitmap.Unlock();
                    Bitmap.Unlock();
                    
                    // Now 'combinedBitmap' contains the combined image of 'bitmap1' and 'bitmap2'
                }
            }
        }

        public override void Stop()
        {
            BodyStream.Stop();
            ColorImageStream.Stop();
        }
    }
}

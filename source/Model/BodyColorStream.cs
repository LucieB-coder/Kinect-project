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

        public KinectSensor KinectSensor { get; private set; }

        private ColorImageStream ColorImageStream;

        public BodyColorStream(KinectManager kinectManager) : base(kinectManager)
        {
            KinectSensor = kinectManager.KinectSensor;
            FrameDescription frameDescription = KinectSensor.ColorFrameSource.CreateFrameDescription(ColorImageFormat.Rgba);
            BodyStream = new BodyStream(kinectManager);
            ColorImageStream = new ColorImageStream(kinectManager);
        }

        public override void Start()
        {
            ColorImageStream.Start();
            BodyStream.Start();

            ColorImageStream.PropertyChanged += Stream_PropertyChanged;
            BodyStream.PropertyChanged += Stream_PropertyChanged;


            FrameDescription frameDescription = KinectSensor.DepthFrameSource.FrameDescription;

            // create the bitmap to display
            Bitmap = new WriteableBitmap(frameDescription.Width, frameDescription.Height, 96, 96, PixelFormats.Bgra32, null);

        }

        private void Stream_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (Bitmap != null && BodyStream.Bitmap != null && ColorImageStream.Bitmap != null) 
            {
                // Suppose bitmap1 and bitmap2 are your two BitmapSource objects
                WriteableBitmap bodyBitmap = BodyStream.Bitmap; // Initialize your first bitmap
                WriteableBitmap colorBitmap = ColorImageStream.Bitmap; // Initialize your second bitmap
                FrameDescription frameDescription = KinectSensor.DepthFrameSource.FrameDescription;


                WriteableBitmap resizedBitmap = new WriteableBitmap(frameDescription.Width, frameDescription.Height, bodyBitmap.DpiX, bodyBitmap.DpiY, bodyBitmap.Format, null);

                using (resizedBitmap.GetBitmapContext())
                {
                    resizedBitmap.Blit(new Rect(0, 0, frameDescription.Width, frameDescription.Height), colorBitmap, new Rect(0, 0, colorBitmap.PixelWidth, colorBitmap.PixelHeight));
                }

                // Dessiner le premier bitmap en arrière-plan
                Bitmap.Lock();
                Bitmap.Blit(new Rect(0, 0, resizedBitmap.PixelWidth, resizedBitmap.PixelHeight),
                        resizedBitmap, new Rect(0, 0, resizedBitmap.PixelWidth, resizedBitmap.PixelHeight));

                // Dessiner le deuxième bitmap par-dessus
                Bitmap.Blit(new Rect(0, 0, bodyBitmap.PixelWidth, bodyBitmap.PixelHeight),
                                    bodyBitmap, new Rect(0, 0, bodyBitmap.PixelWidth, bodyBitmap.PixelHeight));

                Bitmap.Unlock();
                OnPropertyChanged(nameof(Bitmap));
            }
        }

        public override void Stop()
        {
            BodyStream.Stop();
            ColorImageStream.Stop();
        }
    }
}

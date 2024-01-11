using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ViewModel_Wrapper;

namespace TP1
{
    /// <summary>
    /// Logique d'interaction pour MainPage.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        /// <summary>
        /// Size of the RGB pixel in the bitmap
        /// </summary>
        private readonly uint bytesPerPixel;

        /// <summary>
        /// Reader for color frames
        /// </summary>
        private ColorFrameReader colorFrameReader = null;

        /// <summary>
        /// Bitmap to display
        /// </summary>
        private WriteableBitmap bitmap = null;

        /// <summary>
        /// Intermediate storage for receiving frame data from the sensor
        /// </summary>
        private byte[] colorPixels = null;


        public MainWindow()
        {
            KinectManagerViewModel kinectManagerViewModel = new KinectManagerViewModel();
            kinectManagerViewModel.StartSensor();
            // open the reader for the color frames
            this.colorFrameReader = kinectManagerViewModel.KinectSensor.ColorFrameSource.OpenReader();

            // wire handler for frame arrival
            this.colorFrameReader.FrameArrived += this.Reader_ColorFrameArrived;

            // create the colorFrameDescription from the ColorFrameSource using rgba format
            FrameDescription colorFrameDescription = kinectManagerViewModel.KinectSensor.ColorFrameSource.CreateFrameDescription(ColorImageFormat.Rgba);

            // rgba is 4 bytes per pixel
            this.bytesPerPixel = colorFrameDescription.BytesPerPixel;

            // allocate space to put the pixels to be rendered
            this.colorPixels = new byte[colorFrameDescription.Width * colorFrameDescription.Height * this.bytesPerPixel];

            // create the bitmap to display
            this.bitmap = new WriteableBitmap(colorFrameDescription.Width, colorFrameDescription.Height,96,96, PixelFormats.Bgra32, null);

            InitializeComponent();
            DataContext = kinectManagerViewModel;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Handles the color frame data arriving from the sensor.
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void Reader_ColorFrameArrived(object sender, ColorFrameArrivedEventArgs e)
        {
            bool colorFrameProcessed = false;

            // ColorFrame is IDisposable
            using (ColorFrame colorFrame = e.FrameReference.AcquireFrame())
            {
                if (colorFrame != null)
                {
                    FrameDescription colorFrameDescription = colorFrame.FrameDescription;

                    // verify data and write the new color frame data to the Writeable bitmap
                    if ((colorFrameDescription.Width == this.bitmap.PixelWidth) && (colorFrameDescription.Height == this.bitmap.PixelHeight))
                    {
                        if (colorFrame.RawColorImageFormat == ColorImageFormat.Bgra)
                        {
                           // colorFrame.CopyRawFrameDataToArray(this.bitmap.buffer);
                        }
                        else
                        {
                          //  colorFrame.CopyRawFrameDataToArray(this.bitmap.)CopyConvertedFrameDataToBuffer(this.bitmap.PixelBuffer, ColorImageFormat.Bgra);
                        }

                        colorFrameProcessed = true;
                    }
                }
            }

            // we got a frame, render
            if (colorFrameProcessed)
            {
                this.bitmap.Invalidate();
            }
        }

    }
}

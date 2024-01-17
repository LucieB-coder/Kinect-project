using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
using Model;

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
            KinectManager kinectManager = new KinectManager();
            kinectManager.StartSensor();
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
            theImage.Source = this.bitmap;
            DataContext = kinectManager;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Handles the color frame data arriving from the sensor.
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void Reader_ColorFrameArrived(object sender, ColorFrameArrivedEventArgs e)
        {
            using (ColorFrame colorFrame = e.FrameReference.AcquireFrame())
            {
                // If the color frame is null, do nothing
                if (colorFrame != null)
                {
                    // Frame description to retreive the size of the frame
                    FrameDescription colorFrameDescription = colorFrame.FrameDescription;
                    
                    // LockRawImageBuffer is used to retrieve the data of the new frame
                    using (KinectBuffer colorBuffer = colorFrame.LockRawImageBuffer())
                    {
                        // We lock the bitmap to be able to change its content
                        this.bitmap.Lock();
                        // We check that our frame has the same size as our bitmap
                        if ((colorFrameDescription.Width == this.bitmap.PixelWidth) && (colorFrameDescription.Height == this.bitmap.PixelHeight))
                        {
                            // We send the new frame to the buffer of our bitmap 
                            colorFrame.CopyConvertedFrameDataToIntPtr(
                                this.bitmap.BackBuffer,
                                (uint)(colorFrameDescription.Width * colorFrameDescription.Height * 4),
                                ColorImageFormat.Bgra);

                            // The new Invalidate method, specify the rectangle that must be changed in the bitmap (X and Y for the position, Widht and height, well, for widht and height
                            // In our case, we want to invalidate all the bitmap
                            this.bitmap.AddDirtyRect(new Int32Rect
                            {
                                X = 0,
                                Y = 0,
                                Width = this.bitmap.PixelWidth,
                                Height = this.bitmap.PixelHeight
                            });
                        }
                        // We unlock the bitmap to unable the display
                        this.bitmap.Unlock();
                    }
                }
            }
        }
    }
}

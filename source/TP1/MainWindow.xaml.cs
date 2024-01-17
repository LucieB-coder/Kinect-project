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
using Microsoft.Kinect;

namespace TP1
{
    /// <summary>
    /// Logique d'interaction pour MainPage.xaml
    /// </summary>
    public partial class MainWindow : Window
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
            ColorImageStream colorImageStream = new ColorImageStream(kinectManager);
            kinectManager.StartSensor();
            colorImageStream.Start();
            InitializeComponent();
            DataContext = colorImageStream;
        }
    }
}

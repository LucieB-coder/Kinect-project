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
using FirstKinectProject;

namespace TP1
{
    /// <summary>
    /// Logique d'interaction pour MainPage.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private KinectStream? kinectStream; 
        public MainWindow()
        {
            KinectManager kinectManager = new KinectManager();
            KinectStreamsFactory kinectStreamsFactory = new KinectStreamsFactory(kinectManager);
            KinectStreamViewModel kinectStreamViewModel = new KinectStreamViewModel(kinectManager);

            kinectManager.StartSensor();
            kinectStreamViewModel.ChangeStream(KinectStreams.Color);
            InitializeComponent();
            DataContext = kinectStreamViewModel;
        }
    }
}

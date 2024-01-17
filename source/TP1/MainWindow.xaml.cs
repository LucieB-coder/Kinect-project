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
        public KinectManager KinectManager { get; private set; }

        public KinectStreamViewModel KinectStreamViewModel { get; private set; }
        public MainWindow()
        {
            KinectManager = new KinectManager();
            KinectStreamViewModel = new KinectStreamViewModel(KinectManager);
            KinectManager.StartSensor();
            InitializeComponent();
            DataContext = this;
        }
    }
}

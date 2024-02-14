using Model;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace FruitNinja
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public KinectManager KinectManager { get; private set; }
        public GameViewModel GameViewModel { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            Canvas canvas = myCanvas;
            KinectManager = new KinectManager();
            GameViewModel = new GameViewModel(KinectManager, canvas);
            KinectManager.StartSensor();
            DataContext = this;
        }
    }
}

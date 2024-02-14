using KinectUtils;
using Model;
using MyGesturesBank;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FruitNinja
{
    /// <summary>
    /// Logique d'interaction pour MenuWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        public NavigateToGameViewModel NavigateToGameViewModel { get; private set; }

        public MenuWindow()
        {
            InitializeComponent();
            Frame frame = MenuFrame;
            KinectManager kinectManager = new KinectManager();
            NavigateToGameViewModel = new NavigateToGameViewModel(frame, kinectManager, this);
            DataContext = this;
        }
    }
}

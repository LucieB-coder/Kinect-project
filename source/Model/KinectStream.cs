using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Model
{
    public abstract partial class KinectStream : ObservableObject
    {

        public ICommand StartCommand;
        public ICommand StopCommand;


        [ObservableProperty]
        public WriteableBitmap? bitmap;

        protected KinectSensor KinectSensor
        {
            get => KinectManager.KinectSensor;
        }

        public KinectManager KinectManager {get; set;}

        public KinectStream(KinectManager kinectManager)
        {
            KinectManager = kinectManager;
            StartCommand = new RelayCommand(Start);
            StopCommand = new RelayCommand(Stop);
        }

        abstract public void Start();
        abstract public void Stop();
    }
}

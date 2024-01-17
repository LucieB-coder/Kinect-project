using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Model
{
    public abstract partial class KinectStream : ObservableObject
    {

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
        }

        abstract public void Start();
        abstract public void Stop();
    }
}

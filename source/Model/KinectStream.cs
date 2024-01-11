using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public abstract class KinectStream
    {
        protected KinectSensor KinectSensor
        {
            get => KinectManager.KinectSensor;
        }

        protected KinectManager KinectManager {get; set;}

        public KinectStream(KinectManager kinectManager)
        {
            KinectManager = kinectManager;
        }

        abstract public void Start();
        abstract public void Stop();
    }
}

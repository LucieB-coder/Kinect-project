using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class KinectManager
    {
        public KinectSensor KinectSensor { get; set; }
        public KinectManager()
        {
            KinectSensor = KinectSensor.GetDefault();
        }

        public void StartSensor()
        {
            KinectSensor.Open();
        }

        public void StopSensor()
        {
            KinectSensor.Close();
        }
    }
}

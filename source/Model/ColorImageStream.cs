using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ColorImageStream : KinectStream
    {
        public ColorImageStream(KinectManager kinectManager) : base(kinectManager)
        {
        }

        public override void Start()
        {
            KinectSensor.ColorFrameSource.OpenReader();
        }

        public override void Stop()
        {
            KinectSensor.Close();
        }
    }
}

using Microsoft.Kinect;
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

        public ColorFrame? ColorFrame { get; set; }
        public FrameDescription? FrameDescription { get; set; }

        private ColorFrameReader? Reader { get; set; }

        public override void Start()
        {
            Reader = KinectSensor.ColorFrameSource.OpenReader();
            Reader.FrameArrived += FrameArrived;

        }

        private void FrameArrived(object sender, ColorFrameArrivedEventArgs e)
        {
            ColorFrame = e.FrameReference.AcquireFrame();   
        }
        
        public override void Stop()
        {
            ColorFrame.Dispose();

            KinectSensor.Close();
        }
    }
}

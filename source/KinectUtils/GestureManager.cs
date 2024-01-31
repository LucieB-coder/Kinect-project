using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectUtils
{
    public static class GestureManager
    {
        public static EventHandler<GestureRecognizedEventArgs> GestureRecognized;

        public static KinectManager KinectManager { get; set; }

        public static ICollection<BaseGesture> KnownGestures { get; set; } = new List<BaseGesture>();

        public static BodyStream BodyStream { get; set; }

        public static int test { get; set; } = 0;

        public static void StartAcquiringFrame(KinectManager kinectManager)
        {
            //KinectStreamsFactory kinectStreamsFactory = new KinectStreamsFactory(kinectManager);
            BodyStream = new BodyStream(kinectManager);
            GestureRecognized += GestureRecognizedTest;
            BodyStream.PropertyChanged += BodyStream_FrameArrived;
            foreach(BaseGesture g in KnownGestures)
            {
                g.GestureRecognized += GestureRecognized;
            }
            BodyStream.Start();
        }

        public static void StopAcquiringFrame()
        {
            BodyStream.Stop();
        }


        private static void BodyStream_FrameArrived(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            foreach (var gesture in KnownGestures)
            {
                foreach (var body in BodyStream.bodies)
                {
                    gesture.TestGesture(body);
                }
            }
        }

        private static void GestureRecognizedTest(object sender, GestureRecognizedEventArgs e)
        {
            test++;
        }
    }
}

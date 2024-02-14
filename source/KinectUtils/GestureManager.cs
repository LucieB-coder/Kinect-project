using Microsoft.Kinect;
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

        public static EventHandler<BodyFramedArrivedEventArgs> BodyFrameArrived;

        public static KinectManager KinectManager { get; set; }

        public static ObservableCollection<BaseGesture> KnownGestures { get; set; } = new ObservableCollection<BaseGesture>();

        public static BodyStream BodyStream { get; set; }

        public static void StartAcquiringFrame(KinectManager kinectManager)
        {
            //KinectStreamsFactory kinectStreamsFactory = new KinectStreamsFactory(kinectManager);
            KinectManager = kinectManager;
            BodyStream = new BodyStream(kinectManager);
            foreach(BaseGesture g in KnownGestures)
            {
                g.GestureRecognized += GestureRecognized;
            }
            BodyStream.Start();
            BodyStream.kinectSensor.BodyFrameSource.FrameCaptured += BodyStream_FrameArrived;
            KnownGestures.CollectionChanged += KnownGestures_CollectionChanged;
        }

        public static void AddGesture(IGestureFactory factory)
        {
            var list = factory.CreateGestures();
            foreach(var g in list)
            {
                KnownGestures.Add(g);
            }
        }

        public static void AddGesture(BaseGesture gesture)
        {
            KnownGestures.Add(gesture);
        }

        public static void RemoveGesture(BaseGesture gesture)
        {
            KnownGestures.Remove(gesture);
        }

        private static void KnownGestures_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            foreach(BaseGesture b in e.NewItems)
            {
                b.GestureRecognized += GestureRecognized;
            }
            foreach(BaseGesture b in e.OldItems)
            {
                if (b.GestureRecognized != null)
                {
                    b.GestureRecognized -= GestureRecognized;
                }
            }
        }

        public static void StopAcquiringFrame()
        {
            BodyStream.Stop();
            KinectManager.StopSensor();
        }

        private static void BodyStream_FrameArrived(object sender, Microsoft.Kinect.FrameCapturedEventArgs e)
        {
            BodyFrameArrived?.Invoke(sender, new BodyFramedArrivedEventArgs(BodyStream.bodies));

            foreach (var gesture in KnownGestures)
            {
                foreach (var body in BodyStream.bodies)
                {
                    if (body != null)
                    {
                        gesture.TestGesture(body);
                    }
                }
            }
        }

        internal delegate void BodyFramedArrivedEventHandler(object sender, BodyFramedArrivedEventArgs e);
        public class BodyFramedArrivedEventArgs : EventArgs
        {
            public Body[] Bodies { get; set; }

            public BodyFramedArrivedEventArgs(Body[] bodies)
            {
                Bodies = bodies;
            }
        }
    }



}

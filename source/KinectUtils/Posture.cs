using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectUtils
{
    public abstract class Posture : BaseGesture
    {
        public EventHandler<GestureRecognizedEventArgs> GestureUnrecognized { get; set; }

        public Posture(EventHandler<GestureRecognizedEventArgs> gestureRecognized, EventHandler<GestureRecognizedEventArgs> gestureUnrecognized, string gestureName) : base(gestureRecognized, gestureName)
        {
            GestureUnrecognized = gestureUnrecognized;
        }

        protected void OnGestureUnrecognized()
        {
            GestureUnrecognized?.Invoke(this, new GestureRecognizedEventArgs(GestureName));
        }
        protected abstract bool TestPosture(Body body);
    }
}

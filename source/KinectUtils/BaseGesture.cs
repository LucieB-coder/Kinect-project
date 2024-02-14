using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;


namespace KinectUtils
{
    public abstract class BaseGesture
    {
        public EventHandler<GestureRecognizedEventArgs> GestureRecognized { get; set; }

        public string GestureName { get; set; }

        public BaseGesture(EventHandler<GestureRecognizedEventArgs> gestureRecognized, string gestureName)
        {
            GestureRecognized = gestureRecognized;
            GestureName = gestureName;
        }

        public abstract void TestGesture(Body body);

        protected void OnGestureRecognized()
        {
            GestureRecognized?.Invoke(this, new GestureRecognizedEventArgs(GestureName));
        }


    }

    internal delegate void GestureRecognizedEventHandler(object sender, GestureRecognizedEventArgs e);
    public class GestureRecognizedEventArgs : EventArgs
    {
        public string GestureName { get; set; }

        public GestureRecognizedEventArgs( string gestureName )
        {
            GestureName = gestureName;
        }
    }
}

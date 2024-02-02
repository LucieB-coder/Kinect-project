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
        public Posture(EventHandler<GestureRecognizedEventArgs> gestureRecognized, EventHandler<GestureRecognizedEventArgs> gestureUnecognized, string gestureName) : base(gestureRecognized, gestureUnecognized, gestureName)
        {
        }
        protected abstract bool TestPosture(Body body);
    }
}

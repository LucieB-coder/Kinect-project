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
        public Posture(EventHandler<GestureRecognizedEventArgs> gestureRecognized, string gestureName) : base(gestureRecognized, gestureName)
        {
        }
        protected abstract bool TestPosture(Body body);
    }
}

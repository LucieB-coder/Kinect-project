using KinectUtils;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGesturesBank
{
    internal class JoinHandsPosture : Posture
    {
        public JoinHandsPosture(EventHandler<GestureRecognizedEventArgs> gestureRecognized, string gestureName) : base(gestureRecognized, gestureName)
        {
        }

        public override void TestGesture(Body body)
        {
            // Check Posture
            if (TestPosture(body))
            {
                // Send a OnGestureRecognized event
                OnGestureRecognized();
            }
        }

        protected override bool TestPosture(Body body)
        {
            // Get the right hand and left hand joints
            Joint handRight = body.Joints[JointType.HandRight];
            Joint handLeft = body.Joints[JointType.HandLeft];

            // Calculate the distance between left and right hand on the X and Y axis
            float xDistance = Math.Abs(handRight.Position.X - handLeft.Position.X);
            float yDistance = Math.Abs(handRight.Position.Y - handLeft.Position.Y);

            // Check if both hands are joined
            if(xDistance < 1 && yDistance < 1) 
            {
                return true;
            }
            return false;
        }
    }
}

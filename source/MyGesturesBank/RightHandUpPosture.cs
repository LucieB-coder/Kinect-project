using KinectUtils;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGesturesBank
{
    public class RightHandUpPosture : Posture
    {
        public RightHandUpPosture(EventHandler<GestureRecognizedEventArgs> gestureRecognized, string gestureName) : base(gestureRecognized, gestureName)
        {
        }


        public override void TestGesture(Body body)
        {
            // Check Posture
            if (body != null && TestPosture(body))
            {
                // Send a OnGestureRecognized event
                OnGestureRecognized();
            }
        }

        protected override bool TestPosture(Body body)
        {
            // Get the right hand, rught elbow and head joints
            Joint handRight = body.Joints[JointType.HandRight];
            Joint elbowRight = body.Joints[JointType.ElbowRight];
            Joint head = body.Joints[JointType.Head];

            // Check if the right hand is above the right elbow and over the head
            if (handRight.Position.Y > elbowRight.Position.Y && handRight.Position.Y > head.Position.Y)
            {
                return true;
            }

            return false;
        }
    }
}

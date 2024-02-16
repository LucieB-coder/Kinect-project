using KinectUtils;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGesturesBank
{
    public class JoinHandsPosture : Posture
    {
        public JoinHandsPosture(EventHandler<GestureRecognizedEventArgs> gestureRecognized, EventHandler<GestureRecognizedEventArgs> gestureUnecognized, string gestureName) : base(gestureRecognized, gestureUnecognized, gestureName)
        {
        }

        private bool LastGesture = false;
        private int Iteration = 0;

        public override void TestGesture(Body body)
        {
            // Check Posture
            if (body != null && TestPosture(body))
            {
                // Send a OnGestureRecognized event
                if (!LastGesture)
                {
                    OnGestureRecognized();
                    LastGesture = true;
                }
                Iteration = 0;
            }
            else
            {
                if (LastGesture && Iteration == 20)
                {
                    OnGestureUnrecognized();
                    LastGesture = false;
                    Iteration = 0;
                }
                else
                {
                    Iteration++;
                }
            }
            
        }

        protected override bool TestPosture(Body body)
        {
            // Get the right hand and left hand joints
            Joint handRight = body.Joints[JointType.WristRight];
            Joint handLeft = body.Joints[JointType.WristLeft];

            // Calculate the distance between left and right hand on the X and Y axis
            float xDistance = Math.Abs(handRight.Position.X - handLeft.Position.X);
            float yDistance = Math.Abs(handRight.Position.Y - handLeft.Position.Y);

            // Check if both hands are joined
            if(xDistance < 0.15 && yDistance < 0.15 && handRight.TrackingState != TrackingState.NotTracked && handLeft.TrackingState != TrackingState.NotTracked) 
            {
                return true;
            }
            return false;
        }
    }
}

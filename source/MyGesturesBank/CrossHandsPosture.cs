using KinectUtils;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGesturesBank
{
    public class CrossHandsPosture : Posture
    {
        private bool LastGesture = false;
        private int Iteration = 0;

        public CrossHandsPosture(EventHandler<GestureRecognizedEventArgs> gestureRecognized, EventHandler<GestureRecognizedEventArgs> gestureUnecognized, string gestureName) : base(gestureRecognized, gestureUnecognized, gestureName)
        {
        }

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
                if (LastGesture && Iteration == 50)
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
            // Get the required joints
            Joint handRight = body.Joints[JointType.WristRight];
            Joint handLeft = body.Joints[JointType.WristLeft];
            Joint elbowLeft = body.Joints[JointType.ElbowLeft];
            Joint elbowRight = body.Joints[JointType.ElbowRight];

            // Check if both hands are joined
            if (handRight.Position.X < handLeft.Position.X && handLeft.Position.X > elbowLeft.Position.X && handRight.Position.X < elbowRight.Position.X && handRight.Position.Y > elbowLeft.Position.Y && handLeft.Position.Y > elbowRight.Position.Y && handRight.TrackingState != TrackingState.NotTracked && handLeft.TrackingState != TrackingState.NotTracked && elbowLeft.TrackingState != TrackingState.NotTracked && elbowRight.TrackingState != TrackingState.NotTracked)
                    
            {
                return true;
            }
            return false;
        }
    }
}

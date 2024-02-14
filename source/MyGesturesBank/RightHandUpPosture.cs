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
        public RightHandUpPosture(EventHandler<GestureRecognizedEventArgs> gestureRecognized, EventHandler<GestureRecognizedEventArgs> gestureUnecognized, string gestureName) : base(gestureRecognized, gestureUnecognized, gestureName)
        {
        }

        private bool LastGesture = false;
        private int Iteration = 0;


        public override void TestGesture(Body body)
        {
            // Check Posture

            if (body.IsTracked && TestPosture(body))
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
            // Get the right hand, right elbow and head joints
            Joint handRight = body.Joints[JointType.HandRight];
            Joint elbowRight = body.Joints[JointType.ElbowRight];
            Joint head = body.Joints[JointType.Head];

            // Check if the right hand is above the right elbow and over the head
            if (handRight.Position.Y > elbowRight.Position.Y && handRight.Position.Y > head.Position.Y && handRight.TrackingState != TrackingState.NotTracked && elbowRight.TrackingState != TrackingState.NotTracked && head.TrackingState != TrackingState.NotTracked)
            {
                return true;
            }

            return false;
        }
    }
}

using KinectUtils;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGesturesBank
{
    public class SwipeRightHand : Gesture
    {
        public SwipeRightHand(EventHandler<GestureRecognizedEventArgs> gestureRecognized, string gestureName, int minNbOfFrames, int maxNbOfFrames) : base(gestureRecognized, gestureName, minNbOfFrames, maxNbOfFrames)
        {
        }

        private float lastX = 0;
        private int Iteration = 0;

        public override void TestGesture(Body body)
        {
            if (MCurrentFrameCount == 0 && TestInitialConditions(body))
            {
                IsTesting = true;
                MCurrentFrameCount++;
                return;
            }
            if (TestPosture(body) && TestRunningGesture(body))
            {
                Iteration = 0;
                MCurrentFrameCount++;
                if (MCurrentFrameCount >= MinNbOfFrames && MCurrentFrameCount <= MaxNbOfFrames && TestEndCondition(body))
                {
                    lastX = 5;
                    IsTesting = false;
                    MCurrentFrameCount = 0;
                    OnGestureRecognized();
                }
            }
            else
            {
                if (Iteration == 5)
                {
                    MCurrentFrameCount = 0;
                    Iteration = 0;
                    IsTesting = false;
                }
                else
                {
                    Iteration++;
                }
            }
        }

        protected override bool TestEndCondition(Body body)
        {
            Joint handRight = body.Joints[JointType.HandRight];
            Joint shoulder = body.Joints[JointType.ShoulderLeft];
            Joint head = body.Joints[JointType.Head];

            Joint hip = body.Joints[JointType.HipLeft];

            if (handRight.Position.Y < head.Position.Y && handRight.Position.X < shoulder.Position.X && handRight.Position.Y >= hip.Position.Y && hip.TrackingState != TrackingState.NotTracked && handRight.TrackingState != TrackingState.NotTracked && shoulder.TrackingState != TrackingState.NotTracked)
            {
                return true;
            }
            return false;
        }

        protected override bool TestInitialConditions(Body body)
        {
            Joint handRight = body.Joints[JointType.HandRight];
            Joint shoulder = body.Joints[JointType.ShoulderRight];
            Joint head = body.Joints[JointType.Head];


            if (handRight.Position.X > shoulder.Position.X && handRight.Position.Y < head.Position.Y && head.TrackingState != TrackingState.NotTracked && handRight.TrackingState != TrackingState.NotTracked && shoulder.TrackingState != TrackingState.NotTracked)
            {
                lastX = handRight.Position.X;
                return true;
            } 
            return false;
        }

        protected override bool TestPosture(Body body)
        {
            Joint handRight = body.Joints[JointType.HandRight];
            Joint head = body.Joints[JointType.Head];
            Joint hip = body.Joints[JointType.HipLeft];

            if (handRight.Position.Y < head.Position.Y && handRight.Position.Y >= hip.Position.Y && hip.TrackingState != TrackingState.NotTracked && handRight.TrackingState != TrackingState.NotTracked && head.TrackingState != TrackingState.NotTracked)
            {
                return true;
            }
            return false;
        }

        protected override bool TestRunningGesture(Body body)
        {
            Joint handRight = body.Joints[JointType.HandRight];

            if(handRight.Position.X <= lastX)
            {
                lastX = handRight.Position.X;
                return true;
            }
            return false;
        }
    }
}

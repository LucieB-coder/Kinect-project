using KinectUtils;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGesturesBank
{
    public class SwipeLeftHand : Gesture
    {
        public SwipeLeftHand(EventHandler<GestureRecognizedEventArgs> gestureRecognized, string gestureName, int minNbOfFrames, int maxNbOfFrames) : base(gestureRecognized, gestureName, minNbOfFrames, maxNbOfFrames)
        {
        }

        private float lastX = 0;
        private int Iteration = 0;

        public override void TestGesture(Body body)
        {
            if (MCurrentFrameCount == 0)
            {
                if (TestInitialConditions(body))
                {
                    IsTesting = true;
                    MCurrentFrameCount++;
                    return;
                }

            }
            else if (TestPosture(body) && TestRunningGesture(body))
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
            Joint handLeft = body.Joints[JointType.HandLeft];
            Joint shoulder = body.Joints[JointType.ShoulderRight];
            Joint head = body.Joints[JointType.Head];
            Joint hip = body.Joints[JointType.HipRight];

            if (handLeft.Position.Y < head.Position.Y && handLeft.Position.X > shoulder.Position.X && handLeft.Position.Y > hip.Position.Y && hip.TrackingState != TrackingState.NotTracked && handLeft.TrackingState != TrackingState.NotTracked && shoulder.TrackingState != TrackingState.NotTracked)
            {
                return true;
            }
            return false;
        }

        protected override bool TestInitialConditions(Body body)
        {
            Joint handLeft = body.Joints[JointType.HandLeft];
            Joint shoulder = body.Joints[JointType.ShoulderLeft];
            Joint head = body.Joints[JointType.Head];


            if (handLeft.Position.X < shoulder.Position.X && handLeft.Position.Y < head.Position.Y && head.TrackingState != TrackingState.NotTracked && handLeft.TrackingState != TrackingState.NotTracked && shoulder.TrackingState != TrackingState.NotTracked)
            {
                lastX = handLeft.Position.X;
                return true;
            }
            return false;
        }

        protected override bool TestPosture(Body body)
        {
            Joint handLeft = body.Joints[JointType.HandLeft];
            Joint head = body.Joints[JointType.Head];
            Joint hip = body.Joints[JointType.HipLeft];

            if (handLeft.Position.Y < head.Position.Y && handLeft.Position.Y > hip.Position.Y && hip.TrackingState != TrackingState.NotTracked && handLeft.TrackingState != TrackingState.NotTracked && head.TrackingState != TrackingState.NotTracked)
            {
                return true;
            }
            return false;
        }

        protected override bool TestRunningGesture(Body body)
        {
            Joint handLeft = body.Joints[JointType.HandLeft];

            if (handLeft.Position.X > lastX)
            {
                lastX = handLeft.Position.X;
                return true;
            }
            return false;
        }
    }
}

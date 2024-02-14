using KinectUtils;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGesturesBank
{
    public class DiagonalSlashLeftHand : Gesture
    {
        private float lastX = 0;
        private float lastY = 0;
        private int Iteration = 0;

        public DiagonalSlashLeftHand(EventHandler<GestureRecognizedEventArgs> gestureRecognized, string gestureName, int minNbOfFrames, int maxNbOfFrames) : base(gestureRecognized, gestureName, minNbOfFrames, maxNbOfFrames)
        {
        }

        public override void TestGesture(Body body)
        {
            if (MCurrentFrameCount == 0)
            {
                if (body.IsTracked && TestInitialConditions(body))
                {
                    IsTesting = true;
                    MCurrentFrameCount++;
                    return;
                }
            }
            else if (body.IsTracked && TestPosture(body) && TestRunningGesture(body))
            {
                Iteration = 0;
                MCurrentFrameCount++;
                if (MCurrentFrameCount >= MinNbOfFrames && MCurrentFrameCount <= MaxNbOfFrames && TestEndCondition(body))
                {
                    lastX = 5;
                    lastY = 5;
                    IsTesting = false;
                    MCurrentFrameCount = 0;
                    OnGestureRecognized();
                }
            }
            else
            {
                if (Iteration == 20)
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

        protected override bool TestInitialConditions(Body body)
        {
            Joint handLeft = body.Joints[JointType.HandLeft];
            Joint shoulderLeft = body.Joints[JointType.ShoulderLeft];
            Joint elbowLeft = body.Joints[JointType.ElbowLeft];
            Joint head = body.Joints[JointType.Head];

            if (handLeft.Position.X < shoulderLeft.Position.X && handLeft.Position.Y > head.Position.Y && handLeft.Position.Y > elbowLeft.Position.Y && handLeft.Position.X < elbowLeft.Position.X && elbowLeft.Position.X < shoulderLeft.Position.X && head.TrackingState != TrackingState.NotTracked && handLeft.TrackingState != TrackingState.NotTracked && shoulderLeft.TrackingState != TrackingState.NotTracked && elbowLeft.TrackingState != TrackingState.NotTracked)
            {
                lastX = handLeft.Position.X;
                lastY = handLeft.Position.Y;
                return true;
            }
            return false;
        }

        protected override bool TestEndCondition(Body body)
        {
            Joint handLeft = body.Joints[JointType.HandLeft];
            Joint shoulderRight = body.Joints[JointType.ShoulderLeft];
            Joint hipRight = body.Joints[JointType.HipLeft];

            if (handLeft.Position.Y < shoulderRight.Position.Y && handLeft.Position.X > hipRight.Position.X && hipRight.TrackingState != TrackingState.NotTracked && handLeft.TrackingState != TrackingState.NotTracked && shoulderRight.TrackingState != TrackingState.NotTracked)
            {
                return true;
            }
            return false;
        }

        protected override bool TestPosture(Body body)
        {
            return true;
        }

        protected override bool TestRunningGesture(Body body)
        {
            Joint handLeft = body.Joints[JointType.HandLeft];
            if (handLeft.Position.X > lastX && handLeft.Position.Y < lastY)
            {
                lastX = handLeft.Position.X;
                lastY = handLeft.Position.Y;
                return true;
            }
            return false;
        }
    }
}

using KinectUtils;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGesturesBank
{
    public class DiagonalSlashRightHand : Gesture
    {
        private float lastX = 0;
        private float lastY = 0;
        private int Iteration = 0;

        public DiagonalSlashRightHand(EventHandler<GestureRecognizedEventArgs> gestureRecognized, string gestureName, int minNbOfFrames, int maxNbOfFrames) : base(gestureRecognized, gestureName, minNbOfFrames, maxNbOfFrames)
        {
        }

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
            Joint handRight = body.Joints[JointType.HandRight];
            Joint shoulderRight = body.Joints[JointType.ShoulderRight];
            Joint elbowRight = body.Joints[JointType.ElbowRight];
            Joint head = body.Joints[JointType.Head];

            if (handRight.Position.X > shoulderRight.Position.X && handRight.Position.Y > head.Position.Y && handRight.Position.Y > elbowRight.Position.Y && handRight.Position.X > elbowRight.Position.X && elbowRight.Position.X > shoulderRight.Position.X && head.TrackingState != TrackingState.NotTracked && handRight.TrackingState != TrackingState.NotTracked && shoulderRight.TrackingState != TrackingState.NotTracked && elbowRight.TrackingState != TrackingState.NotTracked)
            {
                lastX = handRight.Position.X;
                lastY = handRight.Position.Y;
                return true;
            }
            return false;
        }

        protected override bool TestEndCondition(Body body)
        {
            Joint handRight = body.Joints[JointType.HandRight];
            Joint shoulderLeft = body.Joints[JointType.ShoulderLeft];
            Joint hipLeft = body.Joints[JointType.HipLeft];

            if (body.IsTracked && handRight.Position.Y < shoulderLeft.Position.Y && handRight.Position.X < hipLeft.Position.X && hipLeft.TrackingState != TrackingState.NotTracked && handRight.TrackingState != TrackingState.NotTracked && shoulderLeft.TrackingState != TrackingState.NotTracked)
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
            Joint handRight = body.Joints[JointType.HandRight];
            if (body.IsTracked && handRight.Position.X < lastX && handRight.Position.Y < lastY)
            {
                lastX = handRight.Position.X;
                lastY = handRight.Position.Y;
                return true;
            }
            return false;
        }
    }
}

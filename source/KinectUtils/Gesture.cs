using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectUtils
{
    public abstract class Gesture : BaseGesture
    {
        public bool IsTesting { get; set; } = false;

        protected int MinNbOfFrames { get; set; }

        protected int MaxNbOfFrames { get; set; }

        protected int MCurrentFrameCount { get; set; } = 0;

        public Gesture(EventHandler<GestureRecognizedEventArgs> gestureRecognized, string gestureName, int minNbOfFrames, int maxNbOfFrames) : base(gestureRecognized, gestureName)
        {
            MinNbOfFrames = minNbOfFrames;
            MaxNbOfFrames = maxNbOfFrames;
        }

        abstract protected bool TestInitialConditions(Body body);

        abstract protected bool TestRunningGesture(Body body);

        abstract protected bool TestEndCondition(Body body);

        abstract protected bool TestPosture(Body body);


    }
}

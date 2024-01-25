using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Brush = System.Windows.Media.Brush;
using Brushes = System.Windows.Media.Brushes;
using Color = System.Windows.Media.Color;
using Pen = System.Windows.Media.Pen;

namespace Model
{
    public class BodyStream : KinectStream
    {
        #region Fields

        public KinectSensor kinectSensor;
        private BodyFrameReader bodyFrameReader = null;
        private DrawingGroup drawingGroup;

        // Body size
        private const double HandSize = 10;
        private const double JointThickness = 5;
        private const double ClipBoundThickness = 10;
        private const float InferredZPositionClamp = 0.1f;

        // Colors of the bodies
        private readonly Brush bodiesBrush = Brushes.DarkSalmon;
        private readonly Brush inferBoneBrush = Brushes.Khaki;
        private readonly Pen bodiesPen = new Pen(Brushes.DarkSalmon, 1);
        private readonly Pen inferBonePen = new Pen(Brushes.Khaki, 1);

        // Mapper of coordinates
        private CoordinateMapper coordinateMapper = null;

        // Width and height of the draw
        private int width;
        private int height;

        // Number of bodies on the image
        private Body[] bodies = null;
        // List of bones
        private List<Tuple<JointType, JointType>> bones;

        //Image source displayable on screen
        public ImageSource ImageSource
        {
            get { return new DrawingImage(this.drawingGroup); }
        }

        #endregion

        public BodyStream(KinectManager KinectManager) : base(KinectManager)
        {
            this.kinectSensor = KinectManager.KinectSensor;
            this.drawingGroup = new DrawingGroup();
            this.bodies = new Body[this.kinectSensor.BodyFrameSource.BodyCount];
            Start();
        }

        public override void Start()
        {
            this.KinectManager.StartSensor();

            this.coordinateMapper = this.kinectSensor.CoordinateMapper;
            FrameDescription frameDescription = this.kinectSensor.DepthFrameSource.FrameDescription;
            this.width = frameDescription.Width;
            this.height = frameDescription.Height;
            this.bodyFrameReader = this.kinectSensor.BodyFrameSource.OpenReader();

            if (this.bodyFrameReader != null)
            {
                this.bodyFrameReader.FrameArrived += this.BodyFrameArrived;
            }

            //init des jointures
            this.bones = new List<Tuple<JointType, JointType>>();
            this.bones.Add(new Tuple<JointType, JointType>(JointType.Head, JointType.Neck));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.Neck, JointType.SpineShoulder));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.SpineShoulder, JointType.SpineMid));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.SpineMid, JointType.SpineBase));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.SpineShoulder, JointType.ShoulderRight));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.SpineShoulder, JointType.ShoulderLeft));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.SpineBase, JointType.HipRight));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.SpineBase, JointType.HipLeft));

            this.bones.Add(new Tuple<JointType, JointType>(JointType.ShoulderRight, JointType.ElbowRight));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.ElbowRight, JointType.WristRight));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.WristRight, JointType.HandRight));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.HandRight, JointType.HandTipRight));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.WristRight, JointType.ThumbRight));

            this.bones.Add(new Tuple<JointType, JointType>(JointType.ShoulderLeft, JointType.ElbowLeft));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.ElbowLeft, JointType.WristLeft));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.WristLeft, JointType.HandLeft));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.HandLeft, JointType.HandTipLeft));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.WristLeft, JointType.ThumbLeft));

            this.bones.Add(new Tuple<JointType, JointType>(JointType.HipRight, JointType.KneeRight));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.KneeRight, JointType.AnkleRight));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.AnkleRight, JointType.FootRight));

            this.bones.Add(new Tuple<JointType, JointType>(JointType.HipLeft, JointType.KneeLeft));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.KneeLeft, JointType.AnkleLeft));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.AnkleLeft, JointType.FootLeft));

            // create the bitmap to display
            this.Bitmap = new WriteableBitmap(this.width, this.height, 96, 96, PixelFormats.Bgra32, null);
        }
        private void BodyFrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            bool dataReceived = false;
            using (BodyFrame bodyFrame = e.FrameReference.AcquireFrame())
            {
                if (bodyFrame != null)
                {
                    if (this.bodies == null)
                    {
                        this.bodies = new Body[bodyFrame.BodyCount];
                    }
                    bodyFrame.GetAndRefreshBodyData(this.bodies);
                    dataReceived = true;
                }
            }
            if (dataReceived)
            {
                using (DrawingContext dc = this.drawingGroup.Open())
                {
                    dc.DrawRectangle(Brushes.Black, null, new System.Windows.Rect(0.0, 0.0, this.width, this.height));
                    int penIndex = 0;
                    foreach (Body body in this.bodies)
                    {
                        Pen drawPen = this.bodiesPen;
                        if (body.IsTracked)
                        {
                            this.DrawEdges(body, dc);
                            IReadOnlyDictionary<JointType, Joint> joints = body.Joints;
                            Dictionary<JointType, System.Windows.Point> jointPoints = new Dictionary<JointType, System.Windows.Point>();
                            foreach (JointType jointType in joints.Keys)
                            {
                                CameraSpacePoint pos = joints[jointType].Position;
                                if (pos.Z < 0)
                                {
                                    pos.Z = InferredZPositionClamp;
                                }
                                DepthSpacePoint depthSpacePoint = this.coordinateMapper.MapCameraPointToDepthSpace(pos);
                                jointPoints[jointType] = new System.Windows.Point(depthSpacePoint.X, depthSpacePoint.Y);
                            }
                            this.DrawBody(joints, jointPoints, dc, drawPen);
                            this.DrawHand(body.HandLeftState, jointPoints[JointType.HandLeft], dc);
                            this.DrawHand(body.HandRightState, jointPoints[JointType.HandRight], dc);
                        }
                    }

                    // Set the ClipGeometry on the DrawingGroup
                    this.drawingGroup.ClipGeometry = new RectangleGeometry(new System.Windows.Rect(0.0, 0.0, this.width, this.height));

                    // Create a RenderTargetBitmap
                    RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap(
                        (int)this.width,
                        (int)this.height,
                        96, // DpiX
                        96, // DpiY,
                        PixelFormats.Pbgra32);

                    DrawingVisual drawingVisual = new DrawingVisual();
                    using (DrawingContext drawingContext = drawingVisual.RenderOpen())
                    {
                        drawingContext.DrawDrawing(drawingGroup);
                    }
                    renderTargetBitmap.Render(drawingVisual);

                    // Lock the existing Bitmap
                    this.Bitmap.Lock();

                    try
                    {
                        byte[] pixels = new byte[(int)this.width * (int)this.height * 4]; // 4 bytes per pixel (Pbgra32)
                        renderTargetBitmap.CopyPixels(new Int32Rect(0, 0, (int)this.width, (int)this.height), pixels, (int)this.width * 4, 0);
                        this.Bitmap.WritePixels(new Int32Rect(0, 0, (int)this.width, (int)this.height), pixels, (int)this.width * 4, 0);
                    }
                    finally
                    {
                        // Unlock the existing Bitmap
                        this.Bitmap.Unlock();
                    }
                }
            }
        }

        private void DrawEdges(Body body, DrawingContext drawingContext)
        {
            FrameEdges clippedEdges = body.ClippedEdges;
            if (clippedEdges.HasFlag(FrameEdges.Bottom))
            {
                drawingContext.DrawRectangle(Brushes.Red, null, new System.Windows.Rect(0, this.height - ClipBoundThickness, this.width, ClipBoundThickness));
            }
            if (clippedEdges.HasFlag(FrameEdges.Top))
            {
                drawingContext.DrawRectangle(Brushes.Red, null, new System.Windows.Rect(0, 0, this.width, ClipBoundThickness));
            }
            if (clippedEdges.HasFlag(FrameEdges.Left))
            {
                drawingContext.DrawRectangle(Brushes.Red, null, new System.Windows.Rect(0, 0, ClipBoundThickness, this.height));
            }
            if (clippedEdges.HasFlag(FrameEdges.Right))
            {
                drawingContext.DrawRectangle(Brushes.Red, null, new System.Windows.Rect(this.width - ClipBoundThickness, 0, ClipBoundThickness, this.height));
            }
        }

        private void DrawBody(IReadOnlyDictionary<JointType, Joint> joints, IDictionary<JointType, System.Windows.Point> jointPoints, DrawingContext drawingContext, Pen pen)
        {
            foreach (var bone in this.bones)
            {
                this.DrawBones(joints, jointPoints, bone.Item1, bone.Item2, drawingContext, pen);
            }
            foreach (var joinType in joints.Keys)
            {
                Brush drawBrush = null;
                TrackingState trackingState = joints[joinType].TrackingState;
                if (trackingState == TrackingState.Tracked)
                {
                    drawBrush = this.bodiesBrush;
                }
                else if (trackingState == TrackingState.Inferred)
                {
                    drawBrush = this.inferBoneBrush;
                }
                if (drawBrush != null)
                {
                    drawingContext.DrawEllipse(drawBrush, null, jointPoints[joinType], JointThickness, JointThickness);
                }
            }
        }

        private void DrawHand(HandState handState, System.Windows.Point handPosition, DrawingContext drawingContext)
        {
            switch (handState)
            {
                case HandState.Closed:
                    drawingContext.DrawEllipse(this.bodiesBrush, null, handPosition, HandSize, HandSize);
                    break;
                case HandState.Open:
                    drawingContext.DrawEllipse(this.bodiesBrush, null, handPosition, HandSize, HandSize);
                    break;
                case HandState.Lasso:
                    drawingContext.DrawEllipse(this.bodiesBrush, null, handPosition, HandSize, HandSize);
                    break;
            }
        }

        private void DrawBones(IReadOnlyDictionary<JointType, Joint> joints, IDictionary<JointType, System.Windows.Point> jointPoints, JointType jointType0, JointType jointType1, DrawingContext drawingContext, Pen pen)
        {
            Joint joint0 = joints[jointType0];
            Joint joint1 = joints[jointType1];

            if (joint0.TrackingState == TrackingState.NotTracked || joint1.TrackingState == TrackingState.NotTracked)
            {
                return;
            }
            Pen drawPen = this.inferBonePen;
            if (joint0.TrackingState == TrackingState.Tracked && joint1.TrackingState == TrackingState.Tracked)
            {
                drawPen = pen;
            }
            drawingContext.DrawLine(pen, jointPoints[jointType0], jointPoints[jointType1]);
        }

        public override void Stop()
        {
            if (bodyFrameReader != null)
            {
                bodyFrameReader.Dispose();
            }
        }
    }
}

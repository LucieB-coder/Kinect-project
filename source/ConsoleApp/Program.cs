using KinectUtils;
using Model;
using MyGesturesBank;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        { 
            KinectManager kinectManager = new KinectManager();
            kinectManager.KinectSensor.Open();
            GestureManager.GestureRecognized += GestureRecognizedTest;
            var eventRightHand = new EventHandler<GestureRecognizedEventArgs>(GestureRecognizedHand);
            GestureManager.KnownGestures.Add(new RightHandUpPosture(eventRightHand, "Right Hand"));
            GestureManager.StartAcquiringFrame(kinectManager);
            GestureManager.BodyStream.kinectSensor.BodyFrameSource.FrameCaptured += BodyFrameSource_FrameCaptured; ;
            Console.ReadLine();
            GestureManager.StopAcquiringFrame();
        }

        private static void BodyFrameSource_FrameCaptured(object sender, Microsoft.Kinect.FrameCapturedEventArgs e)
        {
            //Console.WriteLine("Frame arrived");
        }

        private static void GestureRecognizedTest(object sender, GestureRecognizedEventArgs e)
        {
            Console.WriteLine(e.GestureName);
        }

        private static void GestureRecognizedHand(object sender, GestureRecognizedEventArgs e)
        {
            Console.WriteLine(e.GestureName + " Detected");
        }
    }
}

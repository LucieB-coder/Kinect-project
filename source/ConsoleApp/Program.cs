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
            var joinRightHand = new EventHandler<GestureRecognizedEventArgs>(GestureRecognizedJoinHand);

            var eventUnrecognizedRightHand = new EventHandler<GestureRecognizedEventArgs>(GestureUnrecognizedHand);
            var eventUnrecognizedjoinRightHand = new EventHandler<GestureRecognizedEventArgs>(GestureUnrecognizedJoinHand);

            GestureManager.KnownGestures.Add(new RightHandUpPosture(eventRightHand, eventUnrecognizedRightHand, "Right Hand"));
            GestureManager.KnownGestures.Add(new JoinHandsPosture(joinRightHand, eventUnrecognizedjoinRightHand, "Join Hands"));

            GestureManager.StartAcquiringFrame(kinectManager);
            Console.ReadLine();
            GestureManager.StopAcquiringFrame();
        }

        private static void GestureRecognizedTest(object sender, GestureRecognizedEventArgs e)
        {
            //Console.WriteLine(e.GestureName);
        }

        private static void GestureRecognizedHand(object sender, GestureRecognizedEventArgs e)
        {
           Console.WriteLine(e.GestureName + " Detected");
        }

        private static void GestureRecognizedJoinHand(object sender, GestureRecognizedEventArgs e)
        {
            Console.WriteLine(e.GestureName + " Detected");
        }

        private static void GestureUnrecognizedJoinHand(object sender, GestureRecognizedEventArgs e)
        {
            Console.WriteLine(e.GestureName + " Stopped");
        }

        private static void GestureUnrecognizedHand(object sender, GestureRecognizedEventArgs e)
        {
            Console.WriteLine(e.GestureName + " Stopped");
        }
    }
}

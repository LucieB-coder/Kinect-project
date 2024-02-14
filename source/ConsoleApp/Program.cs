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
            //GestureManager.GestureRecognized += GestureRecognizedTest;

            var eventRightHand = new EventHandler<GestureRecognizedEventArgs>(GestureRecognizedHand);
            var joinRightHand = new EventHandler<GestureRecognizedEventArgs>(GestureRecognizedJoinHand);
            var swipeRightHand = new EventHandler<GestureRecognizedEventArgs>(GestureRecognizedSwipeRightHand);
            var swipeLeftHand = new EventHandler<GestureRecognizedEventArgs>(GestureRecognizedSwipeLeftHand);
            var diagonalSlashRightHand = new EventHandler<GestureRecognizedEventArgs>(GestureRecognizedDiagonalSlashRightHand);
            

            var eventUnrecognizedRightHand = new EventHandler<GestureRecognizedEventArgs>(GestureUnrecognizedHand);
            var eventUnrecognizedjoinRightHand = new EventHandler<GestureRecognizedEventArgs>(GestureUnrecognizedJoinHand);

            GestureManager.KnownGestures.Add(new RightHandUpPosture(eventRightHand, eventUnrecognizedRightHand, "Right Hand"));
            GestureManager.KnownGestures.Add(new JoinHandsPosture(joinRightHand, eventUnrecognizedjoinRightHand, "Join Hands"));
            GestureManager.KnownGestures.Add(new SwipeRightHand(swipeRightHand, "Swipe Right Hand", 5, 100));
            GestureManager.KnownGestures.Add(new SwipeLeftHand(swipeLeftHand, "Swipe Left Hand", 5, 100));
            GestureManager.KnownGestures.Add(new DiagonalSlashRightHand(diagonalSlashRightHand, "j't'ai cassé", 5, 1000));


            GestureManager.StartAcquiringFrame(kinectManager);
            Console.ReadLine();
            GestureManager.StopAcquiringFrame();
        }

        private static void GestureRecognizedHand(object sender, GestureRecognizedEventArgs e)
        {
           //Console.WriteLine(e.GestureName + " Detected");
        }

        private static void GestureRecognizedSwipeRightHand(object sender, GestureRecognizedEventArgs e)
        {
            Console.WriteLine(e.GestureName + " Detected");
        }

        private static void GestureRecognizedSwipeLeftHand(object sender, GestureRecognizedEventArgs e)
        {
            Console.WriteLine(e.GestureName + " Detected");
        }

        private static void GestureRecognizedDiagonalSlashRightHand(object sender, GestureRecognizedEventArgs e)
        {
            Console.WriteLine(e.GestureName);
        }

        private static void GestureRecognizedJoinHand(object sender, GestureRecognizedEventArgs e)
        {
           // Console.WriteLine(e.GestureName + " Detected");
        }

        private static void GestureUnrecognizedJoinHand(object sender, GestureRecognizedEventArgs e)
        {
           // Console.WriteLine(e.GestureName + " Stopped");
        }

        private static void GestureUnrecognizedHand(object sender, GestureRecognizedEventArgs e)
        {
           // Console.WriteLine(e.GestureName + " Stopped");
        }
    }
}

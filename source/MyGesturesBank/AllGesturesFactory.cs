using KinectUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGesturesBank
{
    public class AllGesturesFactory : IGestureFactory
    {
        public IEnumerable<BaseGesture> CreateGestures()
        {
            var list = new List<BaseGesture>();
            var eventRightHand = new EventHandler<GestureRecognizedEventArgs>(GestureRecognizedHand);
            var joinRightHand = new EventHandler<GestureRecognizedEventArgs>(GestureRecognizedJoinHand);
            var crossHands = new EventHandler<GestureRecognizedEventArgs>(GestureRecognizedCrossHands);
            var swipeLeftHand = new EventHandler<GestureRecognizedEventArgs>(GestureRecognizedSwipeLeftHand);
            var swipeRightHand = new EventHandler<GestureRecognizedEventArgs>(GestureRecognizedSwipeHand);
            var diagonalSlashRightHand = new EventHandler<GestureRecognizedEventArgs>(GestureRecognizedDiagonalSlashRightHand);
            var diagonalSlashLeftHand = new EventHandler<GestureRecognizedEventArgs>(GestureRecognizedDiagonalSlashLeftHand);

            var eventUnrecognizedRightHand = new EventHandler<GestureRecognizedEventArgs>(GestureUnrecognizedHand);
            var eventUnrecognizedjoinRightHand = new EventHandler<GestureRecognizedEventArgs>(GestureUnrecognizedJoinHand);
            var eventUnrecognizedCrossHands = new EventHandler<GestureRecognizedEventArgs>(GestureUnrecognizedCrossHands);

            list.Add(new RightHandUpPosture(eventRightHand, eventUnrecognizedRightHand, "Right Hand"));
            list.Add(new JoinHandsPosture(joinRightHand, eventUnrecognizedjoinRightHand, "Join Hands"));
            list.Add(new CrossHandsPosture(crossHands, eventUnrecognizedCrossHands, "Cross hands"));
            list.Add(new SwipeRightHand(swipeRightHand, "Swipe Right Hands", 5, 100));
            list.Add(new SwipeLeftHand(swipeLeftHand, "Swipe Left Hand", 5, 100));
            list.Add(new DiagonalSlashRightHand(diagonalSlashRightHand, "j't'ai cassé droite", 5, 200));
            list.Add(new DiagonalSlashLeftHand(diagonalSlashLeftHand, "j't'ai cassé gauche", 5, 200));



            return list;
        }

        private void GestureRecognizedHand(object sender, GestureRecognizedEventArgs e)
        {
            Console.WriteLine(e.GestureName + " Detected");
        }
        private void GestureRecognizedCrossHands(object sender, GestureRecognizedEventArgs e)
        {
            Console.WriteLine(e.GestureName + " Detected");
        }

        private void GestureRecognizedSwipeHand(object sender, GestureRecognizedEventArgs e)
        {
            Console.WriteLine(e.GestureName + " Detected");
        }

        private static void GestureRecognizedSwipeLeftHand(object sender, GestureRecognizedEventArgs e)
        {
            Console.WriteLine(e.GestureName + " Detected");
        }

        private void GestureRecognizedJoinHand(object sender, GestureRecognizedEventArgs e)
        {
            Console.WriteLine(e.GestureName + " Detected");
        }

        private void GestureUnrecognizedJoinHand(object sender, GestureRecognizedEventArgs e)
        {
            Console.WriteLine(e.GestureName + " Stopped");
        }

        private void GestureUnrecognizedCrossHands(object sender, GestureRecognizedEventArgs e)
        {
            Console.WriteLine(e.GestureName + " Stopped");
        }

        private void GestureUnrecognizedHand(object sender, GestureRecognizedEventArgs e)
        {
            Console.WriteLine(e.GestureName + " Stopped");
        }

        private static void GestureRecognizedDiagonalSlashRightHand(object sender, GestureRecognizedEventArgs e)
        {
            Console.WriteLine(e.GestureName + " Detected");
        }

        private static void GestureRecognizedDiagonalSlashLeftHand(object sender, GestureRecognizedEventArgs e)
        {
            Console.WriteLine(e.GestureName + " Detected");
        }
    }
}

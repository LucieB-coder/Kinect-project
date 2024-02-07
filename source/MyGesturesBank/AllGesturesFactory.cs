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
            var swipeRightHand = new EventHandler<GestureRecognizedEventArgs>(GestureRecognizedSwipeHand);


            var eventUnrecognizedRightHand = new EventHandler<GestureRecognizedEventArgs>(GestureUnrecognizedHand);
            var eventUnrecognizedjoinRightHand = new EventHandler<GestureRecognizedEventArgs>(GestureUnrecognizedJoinHand);

            list.Add(new RightHandUpPosture(eventRightHand, eventUnrecognizedRightHand, "Right Hand"));
            list.Add(new JoinHandsPosture(joinRightHand, eventUnrecognizedjoinRightHand, "Join Hands"));
            list.Add(new SwipeRightHand(swipeRightHand, "Swipe Right Hands", 5, 100));
            return list;
        }

        private void GestureRecognizedHand(object sender, GestureRecognizedEventArgs e)
        {
            Console.WriteLine(e.GestureName + " Detected");
        }

        private void GestureRecognizedSwipeHand(object sender, GestureRecognizedEventArgs e)
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

        private void GestureUnrecognizedHand(object sender, GestureRecognizedEventArgs e)
        {
            Console.WriteLine(e.GestureName + " Stopped");
        }
    }
}

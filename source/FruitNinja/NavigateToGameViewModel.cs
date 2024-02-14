using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KinectUtils;
using Model;
using MyGesturesBank;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Navigation;

namespace FruitNinja
{
    public class NavigateToGameViewModel : ObservableObject
    {

        private readonly Frame Frame;

        private int Time = 0;

        private bool Running = false;

        private BaseGesture JoinHandGesture;

        private Window Window { get; set; }

        public NavigateToGameViewModel(Frame frame, KinectManager kinectManager, Window window)
        {
            Frame = frame;
            Window = window;
            BindGestures();
            GestureManager.StartAcquiringFrame(kinectManager);
        }

        private void BindGestures()
        {
            var gestureRecognized = new EventHandler<GestureRecognizedEventArgs>(GestureRecognized);
            var gestureUnrecognized = new EventHandler<GestureRecognizedEventArgs>(GestureUnrecognized);

            JoinHandGesture = new JoinHandsPosture(gestureRecognized, gestureUnrecognized, "joinHands");
            GestureManager.AddGesture(JoinHandGesture);
        }


        private void GestureRecognized(object sender, GestureRecognizedEventArgs e)
        {
            GestureManager.RemoveGesture(JoinHandGesture);
            NavigateToGame();
        }

        private void GestureUnrecognized(object sender, GestureRecognizedEventArgs e)
        {
            Running = false;
            Time = 0;
        }

        public void NavigateToGame()
        {
            var m = new MainWindow();
            m.Show();
            Window.Close();
        }
    }
}

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

            GestureManager.AddGesture(new AllGesturesFactory());

            GestureManager.StartAcquiringFrame(kinectManager);
            Console.ReadLine();
            GestureManager.StopAcquiringFrame();
        }

 
    }
}

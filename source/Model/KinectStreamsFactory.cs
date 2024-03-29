﻿using Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class KinectStreamsFactory
    {
        public KinectStreamsFactory(KinectManager kinectManager)
        {
            streamFactory = new Dictionary<KinectStreams, Func<KinectStream>>
            {
                { KinectStreams.Color, () => new ColorImageStream(kinectManager) },
                { KinectStreams.Depth, () => new DepthStream(kinectManager) },
                { KinectStreams.IR, () => new InfraredStream(kinectManager) },
                { KinectStreams.Body, () => new BodyStream(kinectManager) },
                { KinectStreams.BodyColor, () => new BodyColorStream(kinectManager) },
            };
        }

        private Dictionary<KinectStreams, Func<KinectStream>> streamFactory { get; set; }


        public Func<KinectStream> this[KinectStreams kinectStreams] 
        {
            get => streamFactory[kinectStreams];
        }
    }
}


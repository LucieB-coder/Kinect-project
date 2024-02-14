using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectUtils
{
    public abstract class BaseMapping<T>
    {
        public bool Running { get; set; }
        public EventHandler<GestureRecognizedEventArgs> OnMapping { get; set; }

        public BaseMapping(EventHandler<GestureRecognizedEventArgs> e)
        {
            Running = false;
            OnMapping = e;
        }

        public void SubscribeToStartGesture(BaseGesture gesture)
        {

        }

        public void SubscribeToEndGesture(BaseGesture gesture)
        {

        }

        public void SubscribeToToggleGesture(BaseGesture gesture)
        {

        }

        public abstract T Mapping(Body body);

        protected abstract bool TestMapping(Body body, out T output);

        protected abstract void OnBodyFrameArrived(object o, BodyFrameArrivedEventArgs args);
    }
}

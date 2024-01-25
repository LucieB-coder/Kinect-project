using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FirstKinectProject
{
    public partial class KinectStreamViewModel : ObservableObject
    {
        [ObservableProperty]
        private KinectStream? kinectStream;

        public KinectManager KinectManager { get; set; }

        public RelayCommand<KinectStreams> ChangeStreamCommand { get; set; }

        public KinectStreamsFactory KinectStreamsFactory { get; set; }

        public KinectStreamViewModel(KinectManager kinectManager) 
        { 
            KinectManager = kinectManager;
            KinectStreamsFactory = new KinectStreamsFactory(kinectManager);
            ChangeStreamCommand = new RelayCommand<KinectStreams>(ChangeStream);
        }

        public void ChangeStream(KinectStreams kinectStreams) 
        {
            if (kinectStream != null)
            {
                kinectStream.Stop();
            }
            KinectStream = KinectStreamsFactory[kinectStreams]();
            KinectStream.Start();
        }
    }
}

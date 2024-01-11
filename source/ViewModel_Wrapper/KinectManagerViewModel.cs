using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ViewModel_Wrapper
{

    public partial class KinectManagerViewModel : ObservableObject
    {
        private KinectSensor KinectSensor { get; set; }
        public KinectManagerViewModel()
        {
            KinectSensor = KinectSensor.GetDefault();
            KinectSensor.IsAvailableChanged += KinectSensor_IsAvailableChanged;
        }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(StatusText))]
        private bool status;

        public string StatusText
        {
            get => Status ? "Running" : "Not Running";
        }

        private void KinectSensor_IsAvailableChanged(object sender, IsAvailableChangedEventArgs e)
        {
            // on failure, set the status text
            Status = this.KinectSensor.IsAvailable;
        }


        public void StartSensor()
        {
            KinectSensor.Open();
        }

        public void StopSensor()
        {
            KinectSensor.Close();
        }


    }
}

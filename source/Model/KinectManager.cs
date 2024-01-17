using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Model
{
    public partial class KinectManager : ObservableObject
    {

        public ICommand StartSensorCommand;
        public ICommand StopSensorCommand;

        public KinectSensor KinectSensor { get; set; }
        public KinectManager()
        {
            KinectSensor = KinectSensor.GetDefault();
            KinectSensor.IsAvailableChanged += KinectSensor_IsAvailableChanged;
            StartSensorCommand = new RelayCommand(StartSensor);
            StopSensorCommand = new RelayCommand(StopSensor);
        }

        public void StartSensor()
        {
            KinectSensor.Open();
        }

        public void StopSensor()
        {
            KinectSensor.Close();
        }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(StatusText))]
        [NotifyPropertyChangedFor(nameof(StatusColor))]
        private bool status;

        public string StatusText
        {
            get => Status ? "Running" : "Not Running";
        }


        public string StatusColor
        {
            get => Status ? "Green" : "Red";
        }

        private void KinectSensor_IsAvailableChanged(object sender, IsAvailableChangedEventArgs e)
        {
            // on failure, set the status text
            Status = this.KinectSensor.IsAvailable;
        }
    }
}

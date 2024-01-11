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
using Model;

namespace ViewModel_Wrapper
{

    public partial class KinectManagerViewModel : ObservableObject
    {
        private KinectManager KinectManager;

        public ICommand StartSensorCommand;
        public ICommand StopSensorCommand;

        public KinectManagerViewModel()
        {
            KinectManager = new KinectManager();
            KinectManager.KinectSensor.IsAvailableChanged += KinectSensor_IsAvailableChanged;
            StartSensorCommand = new RelayCommand(StartSensor);
            StopSensorCommand = new RelayCommand(StopSensor);

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
            Status = this.KinectManager.KinectSensor.IsAvailable;
        }


        private void StartSensor()
        {
            KinectManager.StartSensor();
        }

        private void StopSensor()
        {
            KinectManager.StopSensor();
        }


    }
}

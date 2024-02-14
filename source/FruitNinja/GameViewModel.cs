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
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FruitNinja
{

    public partial class GameViewModel : ObservableObject
    {
        [ObservableProperty]
        private KinectStream kinectStream;

        [ObservableProperty]
        public WriteableBitmap? bitmap;

        [ObservableProperty]
        public int score = 0;

        [ObservableProperty]
        public int timer = 60;

        private Canvas Canvas;

        private Image Image;
        private Image Arrow;

        private GestureEnum GestureToExecute;

        public RelayCommand StartGameCommand { get; set; }

        public KinectManager KinectManager { get; set; }

        public GameViewModel(KinectManager kinectManager, Canvas canvas)
        {
            KinectManager = kinectManager;
            kinectStream = new ColorImageStream(kinectManager);
            kinectStream.Start();

            StartGameCommand = new RelayCommand(CreateFruitImage);

            Bitmap = KinectStream.Bitmap;
            KinectStream.PropertyChanged += KinectStream_PropertyChanged;
            BindGestures();
            GestureManager.StartAcquiringFrame(kinectManager);

            Image imgFruit = new Image();
            Canvas = canvas;
            CreateFruitImage();
            Task.Run(() => StartTimer());
        }

        private void StartTimer()
        {
            while (Timer > 0)
            {
                Thread.Sleep(1000);
                Timer -= 1;
            }
        }

        private void BindGestures()
        {
            var gestureRecognized = new EventHandler<GestureRecognizedEventArgs>(GestureRecognized);
   

            GestureManager.AddGesture(new SwipeRightHand(gestureRecognized, "swipeRightHand", 5, 100));
            GestureManager.AddGesture(new SwipeLeftHand(gestureRecognized, "swipeLeftHand", 5, 100));
            GestureManager.AddGesture(new DiagonalSlashRightHand(gestureRecognized, "diagonalSlashRightHand", 5, 200));
            GestureManager.AddGesture(new DiagonalSlashLeftHand(gestureRecognized, "diagonalSlashLeftHand", 5, 200));
        }

        private void GestureRecognized(object sender, GestureRecognizedEventArgs e)
        {
            switch(e.GestureName)
            {
                case "diagonalSlashRightHand":
                    if (GestureToExecute == GestureEnum.DiagonalSlashRightHand)
                    {
                        Score++;
                        CreateFruitImage();
                    }
                    break;
                case "diagonalSlashLeftHand":
                    if (GestureToExecute == GestureEnum.DiagonalSlashLeftHand)
                    {
                        Score++;
                        CreateFruitImage();
                    }
                    break;
                case "swipeRightHand":
                    if (GestureToExecute == GestureEnum.SwipeRightHand)
                    {
                        Score++;
                        CreateFruitImage();
                    }
                    break;
                case "swipeLeftHand":
                    if (GestureToExecute == GestureEnum.SwipeLeftHand)
                    {
                        Score++;
                        CreateFruitImage();
                    }
                    break;
                default:
                    return;
            }
        }




        private void KinectStream_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Bitmap = KinectStream.Bitmap;
        }

        private void CreateFruitImage()
        {
            if (Image != null && Arrow != null)
            {
                Canvas.Children.Remove(Image);
                Canvas.Children.Remove(Arrow);
            }

            Random r = new Random();
            var nb = r.Next(0, 4);

            BitmapImage bitmapImage = new BitmapImage(new Uri("U:\\kinect\\source\\FruitNinja\\res\\PunchingBag.png"));
            BitmapImage bitmapArrow = new BitmapImage();

            switch (nb)
            {
                case 0:
                    GestureToExecute = GestureEnum.DiagonalSlashRightHand;
                    bitmapArrow = new BitmapImage(new Uri("U:\\kinect\\source\\FruitNinja\\res\\RightDiagonnalArrow.png"));
                    break;
                case 1:
                    GestureToExecute = GestureEnum.DiagonalSlashLeftHand;
                    bitmapArrow = new BitmapImage(new Uri("U:\\kinect\\source\\FruitNinja\\res\\LeftDiagonnalArrow.png"));
                    break;
                case 2:
                    GestureToExecute= GestureEnum.SwipeRightHand;
                    bitmapArrow = new BitmapImage(new Uri("U:\\kinect\\source\\FruitNinja\\res\\RightArrow.png"));
                    break;
                case 3:
                    GestureToExecute = GestureEnum.SwipeLeftHand;
                    bitmapArrow = new BitmapImage(new Uri("U:\\kinect\\source\\FruitNinja\\res\\LeftArrow.png"));
                    break;
                default:
                    break;
            }

            Image = new Image();
            Image.Source = bitmapImage;
            Image.Width = 200;
            Image.Height = 100;

            Canvas.SetLeft(Image, 0);
            Canvas.SetTop(Image, 0);

            // Ajoutez l'image à votre Canvas
            Canvas.Children.Add(Image);


            // Déplacer l'image
            Canvas.SetLeft(Image, Canvas.GetLeft(Image) + 100);
            Canvas.SetTop(Image, Canvas.GetTop(Image) + 100);

            Arrow = new Image();

            Arrow.Source = bitmapArrow;
            Arrow.Width = 200;
            Arrow.Height = 100;

            Canvas.SetLeft(Arrow, 0);
            Canvas.SetTop(Arrow, 0);

            // Ajoutez l'image à votre Canvas
            Canvas.Children.Add(Arrow);


            // Déplacer l'image
            Canvas.SetLeft(Arrow, Canvas.GetLeft(Arrow) + 100);
            Canvas.SetTop(Arrow, Canvas.GetTop(Arrow) + 100);
        }
    }
}

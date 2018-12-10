using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using GalagaLite.Class;
using Windows.UI;
using System.Collections.Generic;

namespace GalagaLite
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static CanvasBitmap BG, StartScreen, Level1, ScoreScreen, Photon, Enemy1, Enemy2, SHIP_IMG, MyShip;
        public static Rect bounds = ApplicationView.GetForCurrentView().VisibleBounds;
        public static float DesignWidth = 1920;
        public static float DesignHeight = 1080;
        public static float scaleWidth, scaleHeight, pointX, pointY, photonX, photonY;
        public static int countdown = 60;
        public static bool RoundEnded = false;

        public static int GameState = 0;             //0 refers to StartScreen, 1 to Level1
        public static DispatcherTimer RoundTimer = new DispatcherTimer();
        public static DispatcherTimer EnemyTimer = new DispatcherTimer();

        // Lists (Projectile)
        public static List<float> photonXPOS = new List<float>();
        public static List<float> photonYPOS = new List<float>();
        public static List<float> percent = new List<float>();

        // Lists (Enemies)
        public static List<float> enemyXPOS = new List<float>();
        public static List<float> enemyYPOS = new List<float>();
        public static List<int> enemySHIP = new List<int>();

        //Random Generators
        public Random EnemyShipRand = new Random();     //ship type
        public Random EnemyGenRand = new Random();      //generation interval

        public MainPage()
        {
            this.InitializeComponent();
            Window.Current.SizeChanged += Current_SizeChanged;
            Scaling.SetScale();
            photonX = (float)bounds.Width / 2;
            photonY = (float)bounds.Height;

            RoundTimer.Tick += RoundTimer_Tick;
            RoundTimer.Interval = new TimeSpan(0, 0, 1);       //hours, minutes, seconds

            EnemyTimer.Tick += EnemyTimer_Tick;
            EnemyTimer.Interval = new TimeSpan(0, 0, 0,0, EnemyGenRand.Next(300,3000));


        }

        private void EnemyTimer_Tick(object sender, object e)
        {
            int ES = EnemyShipRand.Next(1, 3);

            enemyXPOS.Add(50 * scaleWidth);
            enemyYPOS.Add(119 * scaleHeight);
            enemySHIP.Add(ES);

            EnemyTimer.Interval = new TimeSpan(0, 0, 0, 0, EnemyGenRand.Next(300, 3000));
        }

        private void RoundTimer_Tick(object sender, object e)
        {
            countdown -= 1;
            if (countdown < 1)
            {
                RoundTimer.Stop();
                RoundEnded = true;
            }
        }

        private void Current_SizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            Scaling.SetScale();
            photonX = (float)bounds.Width / 2;
            photonY = (float)bounds.Height;
        }

        private void GameCanvas_CreateResources(Microsoft.Graphics.Canvas.UI.Xaml.CanvasControl sender, Microsoft.Graphics.Canvas.UI.CanvasCreateResourcesEventArgs args)
        {
            args.TrackAsyncAction(CreateResourcesAsync(sender).AsAsyncAction());
        }

        async Task CreateResourcesAsync(CanvasControl sender)
        {
            StartScreen = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/galaga_logo.png"));
            Level1 = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/star_background.png"));
            ScoreScreen = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/gameover.png"));
            Photon = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/laser.png"));
            MyShip = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/spaceship.png"));
            Enemy1 = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/alien.png"));
            Enemy2 = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/alien2.png"));
        }

        private void GameCanvas_Draw(Microsoft.Graphics.Canvas.UI.Xaml.CanvasControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasDrawEventArgs args)
        {
            gsm.GSM();
            args.DrawingSession.DrawImage(Scaling.img(BG));
            args.DrawingSession.DrawText(countdown.ToString(), 120, 120, Colors.White);

            if(GameState > 0)
            { 
                args.DrawingSession.DrawImage(Scaling.img(MyShip),(float)bounds.Width / 2 - (64 * scaleWidth), (float)bounds.Height - (140 * scaleHeight));

                //Displaying Enemies
                for (int j = 0; j < enemyXPOS.Count; j++)
                {
                    if (enemySHIP[j] == 1) { SHIP_IMG = Enemy1; }
                    if (enemySHIP[j] == 2) { SHIP_IMG = Enemy2; }
                    enemyXPOS[j] += 3;
                    args.DrawingSession.DrawImage(Scaling.img(SHIP_IMG), enemyXPOS[j], enemyYPOS[j]);
                }

                //Display Projectile
                for (int i = 0; i < photonXPOS.Count; i++)
                {
                    pointX = (photonX + (photonXPOS[i] - photonX) * percent[i]);
                    pointY = (photonY + (photonYPOS[i] - photonY) * percent[i]);
                    args.DrawingSession.DrawImage(Scaling.img(Photon), pointX - (41 * scaleWidth), pointY - (63 * scaleHeight));

                    percent[i] += (0.025f) * scaleHeight;

                    if (pointY < 0f)
                    {
                        photonXPOS.RemoveAt(i);
                        photonYPOS.RemoveAt(i);
                        percent.RemoveAt(i);
                    }
                }
            }

            GameCanvas.Invalidate();
        }

        private void GameCanvas_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            if (RoundEnded == true)
            {
                GameState = 0;
                RoundEnded = false;
                countdown = 60;

                //Stop Enemy Timer
                EnemyTimer.Stop();
                enemyXPOS.Clear();
                enemyYPOS.Clear();
                enemySHIP.Clear();
            }
            else
            {
                if (GameState == 0)
                {
                    GameState += 1;
                    RoundTimer.Start();
                    EnemyTimer.Start();

                }
                else if (GameState > 0)
                {
                    photonXPOS.Add((float)e.GetPosition(GameCanvas).X);
                    photonYPOS.Add((float)e.GetPosition(GameCanvas).Y);
                    percent.Add(0f);
                }
            }
        }
    }
}

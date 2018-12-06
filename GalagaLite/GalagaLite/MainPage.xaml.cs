using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.ViewManagement;
using Windows.UI.Core;
using GalagaLite.Class;
using Windows.UI;
using Microsoft.Graphics.Canvas.Text;
using Windows.Storage;
using System.Numerics;
using Windows.System;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace GalagaLite
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static CanvasBitmap BG, StartScreen, Level1, ScoreScreen, Photon, Enemy1, Enemy2, SHIP_IMG, MyShip, Boom;
        public static Rect bounds = ApplicationView.GetForCurrentView().VisibleBounds;
        public static float DesignWidth = 1920;
        public static float DesignHeight = 1080;
        public static float scaleWidth, scaleHeight;
        public static float pointX, pointY;
        public static float photonX, photonY;
        public static float MyScore, boomX, boomY;
        public static float ShipXPOS = 0, ShipYPOS;
        public static int boomCount = 60;
        public static int countdown = 20;
        public static bool RoundEnded = false;

        public static int GameState = 0;
        public static string STRHighScore = "0";

        public static DispatcherTimer RoundTimer = new DispatcherTimer();
        public static DispatcherTimer EnemyTimer = new DispatcherTimer();
        //Lists (Projectile)
        public static List<float> photonXPOS = new List<float>();
        public static List<float> photonYPOS = new List<float>();
        public static List<float> percent = new List<float>();
        public static List<float> photonXPOSs = new List<float>();
        public static List<float> photonYPOSs = new List<float>();

        //Lists (Enemies)
        public static List<float> enemyXPOS = new List<float>();
        public static List<float> enemyYPOS = new List<float>();
        public static List<int> enemySHIP = new List<int>();
        public static List<string> enemyDIR = new List<string>();

        //Random Number Generators
        public Random EnemyShipRand = new Random();
        public Random EnemyGenRand = new Random();
        public Random EnemyXstart = new Random();

        //storage folder


        public MainPage()
        {
            this.InitializeComponent();
            Window.Current.SizeChanged += Current_SizeChanged;
            Scaling.SetScale();
            photonX = (float)bounds.Width / 2;
            photonY = (float)bounds.Height;
            RoundTimer.Tick += RoundTimer_Tick;
            RoundTimer.Interval = new TimeSpan(0, 0, 1);

            EnemyTimer.Tick += EnemyTimer_Tick;
            EnemyTimer.Interval = new TimeSpan(0, 0, 0, 0, EnemyGenRand.Next(300, 3000));



            Storage.CreateFile();
            Storage.ReadFile();

            ShipXPOS = (float)bounds.Width / 2 - (65 * scaleWidth);
            ShipYPOS = (float)bounds.Height - (130 * scaleHeight);
        }
        private void EnemyTimer_Tick(object sender, object e)
        {
            int ES = EnemyShipRand.Next(1, 3);
            int SP = EnemyXstart.Next(0, (int)bounds.Width);
            if (SP > bounds.Width / 2)
            {
                enemyDIR.Add("left");
            }
            else
            {
                enemyDIR.Add("right");
            }

            enemyXPOS.Add(SP);
            enemyYPOS.Add(-50 * scaleHeight);
            enemySHIP.Add(ES);

            EnemyTimer.Interval = new TimeSpan(0, 0, 0, 0, EnemyGenRand.Next(303, 2000));
        }
        void Grid_KeyDown(object sender, KeyRoutedEventArgs e)
        {

            if (e.Key == VirtualKey.Right)
            {
                ShipXPOS = ShipXPOS + 3;
            }
            if (e.Key == VirtualKey.Left)
            {
                ShipXPOS = ShipXPOS - 3;
            }
        }
        private void RoundTimer_Tick(object sender, object e)
        {
            countdown -= 1;

            if (countdown < 1)
            {
                RoundTimer.Stop();
                RoundEnded = true;
                MyScore = 0;
            }
        }

        private void Current_SizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            bounds = ApplicationView.GetForCurrentView().VisibleBounds;
            Scaling.SetScale();
            //photonX = (float)bounds.Width / 2;
            //photonY = (float)bounds.Height;
        }

        private void GameCanvas_CreateResources(Microsoft.Graphics.Canvas.UI.Xaml.CanvasControl sender, Microsoft.Graphics.Canvas.UI.CanvasCreateResourcesEventArgs args)
        {
            args.TrackAsyncAction(CreateResourcesAsync(sender).AsAsyncAction());
        }

        async Task CreateResourcesAsync(CanvasControl sender)
        {
            StartScreen = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/background1.jpg"));
            Level1 = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/background2.jpg"));
            ScoreScreen = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/scorescreen.jpg"));
            MyShip = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/Space Invaders Icon2.png"));
            Photon = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/beam.jpg"));
            Enemy1 = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/space2.jpg"));
            Enemy2 = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/green.png"));
            Boom = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/boom.JPG"));

        }
        private void GameCanvas_Draw(Microsoft.Graphics.Canvas.UI.Xaml.CanvasControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasDrawEventArgs args)
        {
            GSM.gamelevel();
            args.DrawingSession.DrawImage(Scaling.img(BG));
            args.DrawingSession.DrawText(countdown.ToString(), 100, 100, Colors.Yellow);
            if (RoundEnded == true)
            {
                //if(MyScore > Convert.ToInt16(STRHighScore))
                //{
                Storage.UpdateScore();
                //} 


                CanvasTextLayout textLayout1 = new CanvasTextLayout(args.DrawingSession, MyScore.ToString(), new CanvasTextFormat() { FontSize = (36 * scaleHeight), WordWrapping = CanvasWordWrapping.NoWrap }, 0.0f, 0.0f);
                args.DrawingSession.DrawTextLayout(textLayout1, ((DesignWidth * scaleWidth) / 2) - ((float)textLayout1.DrawBounds.Width / 2), 480 * scaleHeight, Colors.White);
                args.DrawingSession.DrawText("Highscore: " + Convert.ToInt16(STRHighScore), new Vector2(200, 200), Color.FromArgb(255, 200, 150, 210));

            }
            else
            {
                if (GameState > 0)
                {
                    args.DrawingSession.DrawText("Score: " + MyScore.ToString(), (float)bounds.Width / 2, 10, Color.FromArgb(255, 255, 255, 255));
                    if (boomX > 0 && boomY > 0 && boomCount > 0)
                    {
                        args.DrawingSession.DrawImage(Scaling.img(Boom), boomX, boomY);
                        boomCount--;
                    }
                    else
                    {
                        boomCount = 60;
                        boomX = 0;
                        boomY = 0;
                    }

                    //Enemies
                    for (int j = 0; j < enemyXPOS.Count; j++)
                    {
                        if (enemySHIP[j] == 1)
                        {
                            SHIP_IMG = Enemy1;
                        }
                        if (enemySHIP[j] == 2)
                        {
                            SHIP_IMG = Enemy2;
                        }
                        if (enemyDIR[j] == "left")
                        {
                            enemyXPOS[j] -= 3;
                        }
                        if (enemyDIR[j] == "right")
                        {
                            enemyXPOS[j] += 3;
                        }
                        enemyYPOS[j] += 3;
                        args.DrawingSession.DrawImage(Scaling.img(SHIP_IMG), enemyXPOS[j], enemyYPOS[j]);

                    }
                    //Display Projectiles
                    for (int i = 0; i < photonXPOS.Count; i++)
                    {
                        pointX = (photonXPOSs[i] + (photonXPOS[i] - photonXPOSs[i]) * percent[i]);
                        pointY = (photonYPOSs[i] + (photonYPOS[i] - photonYPOSs[i]) * percent[i]);

                        args.DrawingSession.DrawImage(Scaling.img(Photon), pointX - (12 * scaleWidth), pointY - (25 * scaleHeight));


                        percent[i] += (0.050f * scaleHeight);

                        for (int h = 0; h < enemyXPOS.Count; h++)
                        {
                            if (pointX >= enemyXPOS[h] && pointX <= enemyXPOS[h] + (74 * scaleWidth) && pointY >= enemyYPOS[h] && pointY <= enemyYPOS[h] + (66 * scaleHeight))
                            {
                                boomX = pointX - (37 * scaleWidth);
                                boomY = pointY - (33 * scaleHeight);

                                enemyXPOS.RemoveAt(h);
                                enemyYPOS.RemoveAt(h);
                                enemySHIP.RemoveAt(h);
                                enemyDIR.RemoveAt(h);

                                photonXPOS.RemoveAt(i);
                                photonYPOS.RemoveAt(i);
                                photonXPOSs.RemoveAt(i);
                                photonYPOSs.RemoveAt(i);
                                percent.RemoveAt(i);

                                MyScore = MyScore + 100;

                                break;
                            }
                        }



                        if (pointY < 0f)
                        {
                            photonXPOS.RemoveAt(i);
                            photonYPOS.RemoveAt(i);
                            photonXPOSs.RemoveAt(i);
                            photonYPOSs.RemoveAt(i);
                            percent.RemoveAt(i);
                        }
                    }
                    args.DrawingSession.DrawImage(Scaling.img(MyShip), ShipXPOS, ShipYPOS);
                }
            }

            GameCanvas.Invalidate();
        }

        private void GameCanvas_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (RoundEnded == true)
            {
                GameState = 0;
                RoundEnded = false;
                countdown = 6;

                EnemyTimer.Stop();
                enemyXPOS.Clear();
                enemyYPOS.Clear();
                enemySHIP.Clear();
                enemyDIR.Clear();
            }

            else
            {
                if (GameState == 0)
                {
                    GameState++;
                    RoundTimer.Start();
                    EnemyTimer.Start();
                }
                else if (GameState > 0)
                {
                    photonXPOSs.Add((float)(ShipXPOS + (MyShip.Bounds.Width * scaleWidth / 2)));
                    photonYPOSs.Add((float)bounds.Height - (65 * scaleHeight));
                    photonXPOS.Add((float)e.GetPosition(GameCanvas).X);
                    photonYPOS.Add((float)e.GetPosition(GameCanvas).Y);
                    percent.Add(0f);
                }
            }

        }
    }
}

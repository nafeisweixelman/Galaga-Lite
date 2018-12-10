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
using Microsoft.Graphics.Canvas.Text;
using System.Numerics;

namespace GalagaLite
{
    public sealed partial class MainPage : Page
    {
        public static CanvasBitmap BG, StartScreen, Level1, ScoreScreen, Photon, Enemy1, Enemy2, SHIP_IMG, MyShip, Boom, Rules;
        public static Rect bounds = ApplicationView.GetForCurrentView().VisibleBounds;
        public static float DesignWidth = 1920;
        public static float DesignHeight = 1080;
        public static float scaleWidth, scaleHeight, pointX, pointY, photonX, photonY, MyScore, boomX, boomY;
        public static int boomCount = 50;
        public static int countdown = 25;
        public static bool RoundEnded = false;

        //High Score
        public static string STRHighScore = "0";

        public static int GameState = 0;
        public static DispatcherTimer RoundTimer = new DispatcherTimer();
        public static DispatcherTimer EnemyTimer = new DispatcherTimer();

        //Lists (Projectile)
        public static List<float> photonXPOS = new List<float>();
        public static List<float> photonYPOS = new List<float>();
        public static List<float> percent = new List<float>();

        //Lists (Enemies)
        public static List<float> enemyXPOS = new List<float>();
        public static List<float> enemyYPOS = new List<float>();
        public static List<int> enemySHIP = new List<int>();
        public static List<string> enemyDIR = new List<string>();

        //Random Generators
        public Random EnemyShipRand = new Random();     //ship type
        public Random EnemyGenRand = new Random();      //generation interval
        public Random EnemyXstart = new Random();

        //Fonts
        public static CanvasTextFormat textFormat1 = new CanvasTextFormat()
        {
            FontSize = 60, WordWrapping = CanvasWordWrapping.NoWrap
        };

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
            EnemyTimer.Interval = new TimeSpan(0, 0, 0, 0, EnemyGenRand.Next(300,2000));

            Storage.CreateFile();
            Storage.ReadFile();
        }

        private void EnemyTimer_Tick(object sender, object e)
        {
            int ES = EnemyShipRand.Next(1, 3);
            int SP = EnemyXstart.Next(0, (int) bounds.Width);
            
            if(SP > bounds.Width /2)
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

            EnemyTimer.Interval = new TimeSpan(0, 0, 0, 0, EnemyGenRand.Next(500, 2000));
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
            bounds = ApplicationView.GetForCurrentView().VisibleBounds;
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
            Rules = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/rules.png"));
            Photon = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/laser.png"));
            MyShip = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/spaceship.png"));
            Enemy1 = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/alien.png"));
            Enemy2 = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/alien2.png"));
            Boom = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/boom.png"));
        }

        private void GameCanvas_Draw(Microsoft.Graphics.Canvas.UI.Xaml.CanvasControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasDrawEventArgs args)
        {
            gsm.GSM();
            args.DrawingSession.DrawImage(Scaling.img(BG));
            args.DrawingSession.DrawText(countdown.ToString(), 120, 120, Colors.White);

            if(RoundEnded == true)
            {
   
                Storage.UpdateScore();
                
                CanvasTextLayout textLayout1 = new CanvasTextLayout(args.DrawingSession, MyScore.ToString(), textFormat1, 0.0f, 0.0f);
                args.DrawingSession.DrawTextLayout(textLayout1, ((DesignWidth * scaleWidth) / 2) - ((float)textLayout1.DrawBounds.Width / 2 - 20), 816 * scaleHeight, Colors.White);
                args.DrawingSession.DrawText("HighScore: " + Convert.ToInt16(STRHighScore), new Vector2(200,200), Color.FromArgb(255,200,150,210));
            }
            else
            {
                if (GameState > 1)
                {
                    args.DrawingSession.DrawText("Score: " + MyScore.ToString(), (float)bounds.Width / 2, 50, Color.FromArgb(255, 255, 255, 255));

                    //Explosion
                    if (boomX > 0 && boomY > 0 && boomCount > 0)
                    {
                        args.DrawingSession.DrawImage(Scaling.img(Boom), boomX, boomY);
                        boomCount -= 1;
                    }
                    else
                    {
                        boomCount = 50;
                        boomX = 0;
                        boomY = 0;
                    }

                    //Displaying Enemies
                    for (int j = 0; j < enemyXPOS.Count; j++)
                    {
                        if (enemySHIP[j] == 1) { SHIP_IMG = Enemy1; }
                        if (enemySHIP[j] == 2) { SHIP_IMG = Enemy2; }

                        if (enemyDIR[j] == "left")
                        {
                            enemyXPOS[j] -= 3;
                        }
                        else
                        {
                            enemyXPOS[j] += 3;
                        }
                        enemyYPOS[j] += 3;
                        args.DrawingSession.DrawImage(Scaling.img(SHIP_IMG), enemyXPOS[j], enemyYPOS[j]);
                    }

                    //Display Projectile
                    for (int i = 0; i < photonXPOS.Count; i++)
                    {
                        pointX = (photonX + (photonXPOS[i] - photonX) * percent[i]);
                        pointY = (photonY + (photonYPOS[i] - photonY) * percent[i]);
                        args.DrawingSession.DrawImage(Scaling.img(Photon), pointX - (41 * scaleWidth), pointY - (63 * scaleHeight));

                        percent[i] += (0.1f) * scaleHeight;

                        for (int h = 0; h < enemyXPOS.Count; h++)
                        {
                            if (pointX >= enemyXPOS[h] && pointX <= enemyXPOS[h] + (130 * scaleWidth) && pointY >= enemyYPOS[h] && pointY <= enemyYPOS[h] + (120 * scaleHeight))
                            {
                                boomX = pointX - (65 * scaleWidth);
                                boomY = pointY - (60 * scaleHeight);

                                enemyXPOS.RemoveAt(h);
                                enemyYPOS.RemoveAt(h);
                                enemySHIP.RemoveAt(h);
                                enemyDIR.RemoveAt(h);

                                photonXPOS.RemoveAt(i);
                                photonYPOS.RemoveAt(i);
                                percent.RemoveAt(i);

                                MyScore = MyScore + 100;

                                break;
                            }
                        }

                        if (pointY < 0f)
                        {
                            photonXPOS.RemoveAt(i);
                            photonYPOS.RemoveAt(i);
                            percent.RemoveAt(i);
                        }
                    }
                    args.DrawingSession.DrawImage(Scaling.img(MyShip), (float)bounds.Width / 2 - (64 * scaleWidth), (float)bounds.Height - (140 * scaleHeight));
                }
            }
                

            GameCanvas.Invalidate();
        }

        private void GameCanvas_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            if (RoundEnded == true)
            {
                if( ((float)e.GetPosition(GameCanvas).X > 735 * scaleWidth && (float)e.GetPosition(GameCanvas).X < 1176 * scaleWidth) && (float)e.GetPosition(GameCanvas).Y > 940 * scaleHeight && (float)e.GetPosition(GameCanvas).Y < 1005 * scaleHeight)
                {
                    GameState = 0;
                    RoundEnded = false;
                    countdown = 25;

                    //Stop Enemy Timer
                    EnemyTimer.Stop();
                    enemyXPOS.Clear();
                    enemyYPOS.Clear();
                    enemySHIP.Clear();
                    enemyDIR.Clear();
                    MyScore = 0;
                }

            }
            else
            {
                if (GameState != 2)
                {
                    if (((float)e.GetPosition(GameCanvas).X > 1102 * scaleWidth && (float)e.GetPosition(GameCanvas).X < 1383 * scaleWidth) && (float)e.GetPosition(GameCanvas).Y > 803 * scaleHeight && (float)e.GetPosition(GameCanvas).Y < 870 * scaleHeight)
                    {
                        GameState = 1;
                    }

                    if (((float)e.GetPosition(GameCanvas).X > 258 * scaleWidth && (float)e.GetPosition(GameCanvas).X < 624 * scaleWidth) && (float)e.GetPosition(GameCanvas).Y > 670 * scaleHeight && (float)e.GetPosition(GameCanvas).Y < 805 * scaleHeight)
                    {
                        GameState = 0;
                    }

                    if (((float)e.GetPosition(GameCanvas).X > 546 * scaleWidth && (float)e.GetPosition(GameCanvas).X < 826 * scaleWidth) && (float)e.GetPosition(GameCanvas).Y > 799 * scaleHeight && (float)e.GetPosition(GameCanvas).Y < 865 * scaleHeight)
                    {
                        GameState = 2;
                        RoundTimer.Start();
                        EnemyTimer.Start();
                    }
                }
                else if (GameState > 1)
                {
                    photonXPOS.Add((float)e.GetPosition(GameCanvas).X);
                    photonYPOS.Add((float)e.GetPosition(GameCanvas).Y);
                    percent.Add(0f);
                }
            }
        }
    }
}

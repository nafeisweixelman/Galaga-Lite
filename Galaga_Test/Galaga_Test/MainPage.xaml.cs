using Galaga_Test.Class;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using System.Numerics;

namespace Galaga_Test
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static CanvasBitmap boom, backGround, startScreen, level1, scoreScreen, laser, enemy1, enemy2, SHIP_IMG, myShip;
        public static Rect bounds = ApplicationView.GetForCurrentView().VisibleBounds;
        public static float DesignWidth = 1280;
        public static float DesignHeight = 720;
        public static float scaleWidth, scaleHeight, pointX, pointY, laserX, laserY, myScore, boomX, boomY;
        public static int boomCount = 60;
        public static int gameState = 0;
        public static int gametime = 10, countdown = gametime;
        public static bool roundEnded = false;
        public static List<float> laserXPOS = new List<float>();
        public static List<float> laserYPOS = new List<float>();
        public static List<float> percent = new List<float>();
        public static List<float> enemyXPOS = new List<float>();
        public static List<float> enemyYPOS = new List<float>();
        public static List<int> enemySHIP = new List<int>();
        public static List<string> enemyDIR = new List<string>();

        public Random enemyShipRand = new Random();
        public Random enemyGenRand = new Random();
        public Random enemyXStart = new Random();

        StorageFolder storage = ApplicationData.Current.LocalFolder;
        public static string strHighScore;
        public static int highScore;

        public static DispatcherTimer roundTimer = new DispatcherTimer();
        public static DispatcherTimer enemyTimer = new DispatcherTimer();

        public MainPage()
        {
            this.InitializeComponent();

            Window.Current.SizeChanged += Current_SizeChanged;
            Scaling.SetScale();
            laserX = (float)bounds.Width / 2;
            laserY = (float)bounds.Height;
            roundTimer.Tick += roundTimer_Tick;
            roundTimer.Interval = new TimeSpan(0, 0, 1);

            enemyTimer.Tick += EnemyTimer_Tick;
            enemyTimer.Interval = new TimeSpan(0, 0, 0, 0, enemyGenRand.Next(300, 3000));

            Storage.CreateFile();
            Storage.ReadFile();
        }


        private void EnemyTimer_Tick(object sender, object e)
        {
            int ES = enemyShipRand.Next(1, 3);
            int SP = enemyXStart.Next(0, (int)bounds.Width);

            if (SP > bounds.Width / 2)
                enemyDIR.Add("left");
            else
                enemyDIR.Add("right");

            enemyXPOS.Add(SP);
            enemyYPOS.Add(-50 * scaleHeight);
            enemySHIP.Add(ES);

            enemyTimer.Interval = new TimeSpan(0, 0, 0, 0, enemyGenRand.Next(300, 3000));
        }

        private void roundTimer_Tick(object sender, object e)
        {
            countdown--;

            if (countdown < 1)
            {
                roundTimer.Stop();
                roundEnded = true;
            }
        }

        private void Current_SizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            bounds = ApplicationView.GetForCurrentView().VisibleBounds;
            Scaling.SetScale();

            laserX = (float)bounds.Width / 2;
            laserY = (float)bounds.Height;
        }

        private void GameCanvas_CreateResources(Microsoft.Graphics.Canvas.UI.Xaml.CanvasControl sender, Microsoft.Graphics.Canvas.UI.CanvasCreateResourcesEventArgs args)
        {
            args.TrackAsyncAction(CreateResourcesAsync(sender).AsAsyncAction());
        }

        async Task CreateResourcesAsync(CanvasControl sender)
        {
            startScreen = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/universo.jpg"));
            level1 = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/universo.jpg"));
            scoreScreen = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/image.png"));
            laser = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/blue.png"));
            enemy1 = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/image.png"));
            enemy2 = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/blue.png"));
            myShip = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/blue.png"));
            boom = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/blue.png"));
        }
        private void GameCanvas_Draw(Microsoft.Graphics.Canvas.UI.Xaml.CanvasControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasDrawEventArgs args)
        {
            GSM.Gsm();
            args.DrawingSession.DrawImage(Scaling.img(backGround));
            args.DrawingSession.DrawText(countdown.ToString(), 100, 100, Colors.Yellow);

            if (roundEnded == true)
            {
                Storage.UpdateScore();

                CanvasTextLayout textLayout = new CanvasTextLayout(args.DrawingSession, myScore.ToString(), new CanvasTextFormat() { FontSize = (36 * scaleWidth), WordWrapping = CanvasWordWrapping.NoWrap }, 0.0f, 0.0f);
                args.DrawingSession.DrawTextLayout(textLayout, ((DesignWidth * scaleWidth) / 2) - ((float)textLayout.DrawBounds.Width / 2), 480 * scaleHeight, Colors.Yellow);
                args.DrawingSession.DrawText("HighScore: " + Convert.ToInt16(strHighScore), new Vector2(200, 200), Color.FromArgb(255, 200, 150, 210));
            }

            else
            {
                if (gameState > 0)
                {
                    args.DrawingSession.DrawText("Score: " + myScore.ToString(), (float)bounds.Width / 2, 10, Color.FromArgb(255, 255, 255, 255));

                    if (boomX > 0 && boomY > 0 && boomCount > 0)
                    {
                        args.DrawingSession.DrawImage(Scaling.img(boom), boomX, boomY);
                        boomCount -= 1;
                    }
                    else
                    {
                        boomCount = 60;
                        boomX = 0;
                        boomY = 0;
                    }

                    for (int j = 0; j < enemyXPOS.Count; j++)
                    {
                        if (enemySHIP[j] == 1)
                            SHIP_IMG = enemy1;
                        if (enemySHIP[j] == 2)
                            SHIP_IMG = enemy2;
                        if (enemyDIR[j] == "left")
                            enemyXPOS[j] -= 3;
                        else
                            enemyXPOS[j] += 3;

                        enemyYPOS[j] += 3;
                        args.DrawingSession.DrawImage(Scaling.img(SHIP_IMG), enemyXPOS[j], enemyYPOS[j]);
                    }


                    for (int i = 0; i < laserXPOS.Count; i++)
                    {
                        pointX = (laserX + (laserXPOS[i] - laserX) * percent[i]);
                        pointY = (laserY + (laserYPOS[i] - laserY) * percent[i]);

                        args.DrawingSession.DrawImage(Scaling.img(laser), pointX - (34 * scaleWidth), pointY - (34 * scaleHeight));

                        percent[i] += (0.050f * scaleHeight);

                        for (int h = 0; h < enemyXPOS.Count; h++)
                        {
                            if (pointX >= enemyXPOS[h] && pointX <= enemyXPOS[h] + (50 * scaleWidth) && pointY >= enemyYPOS[h] && pointY <= enemyYPOS[h] + (50 * scaleHeight))
                            {
                                boomX = pointX - (25 * scaleWidth);
                                boomY = pointY - (25 * scaleWidth);

                                enemyXPOS.RemoveAt(h);
                                enemyYPOS.RemoveAt(h);
                                enemySHIP.RemoveAt(h);
                                enemyDIR.RemoveAt(h);

                                laserXPOS.RemoveAt(i);
                                laserYPOS.RemoveAt(i);
                                percent.RemoveAt(i);

                                myScore = myScore + 100;

                                break;
                            }
                        }

                        if (pointY < 0f)
                        {
                            laserXPOS.RemoveAt(i);
                            laserYPOS.RemoveAt(i);
                            percent.RemoveAt(i);
                        }
                    }

                    args.DrawingSession.DrawImage(Scaling.img(myShip), (float)bounds.Width / 2 - (46 * scaleWidth), (float)bounds.Height - (115 * scaleHeight));
                }
            }

            GameCanvas.Invalidate();
        }

        private void GameCanvas_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (roundEnded == true)
            {
                gameState = 0;
                roundEnded = false;
                countdown = gametime;
                myScore = 0;

                enemyTimer.Stop();
                enemyXPOS.Clear();
                enemyYPOS.Clear();
                enemySHIP.Clear();
                enemyDIR.Clear();

            }
            else
            {
                if (gameState == 0)
                {
                    gameState++;
                    roundTimer.Start();
                    enemyTimer.Start();
                }
                else if (gameState > 0)
                {
                    laserXPOS.Add((float)e.GetPosition(GameCanvas).X);
                    laserYPOS.Add((float)e.GetPosition(GameCanvas).Y);
                    percent.Add(0f);
                }
            }
        }
    }
}

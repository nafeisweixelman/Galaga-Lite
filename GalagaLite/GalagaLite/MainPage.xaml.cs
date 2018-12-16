using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.ViewManagement;
using Windows.UI.Core;
using GalagaLite.Class;
using Windows.UI;
using Microsoft.Graphics.Canvas.Text;
using System.Numerics;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace GalagaLite
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static CanvasBitmap BG, Rules, StartScreen, Level1, GameOver, Continue, Photon, Enemy1, Enemy2, ALIEN_IMG, MyShip, Boom, Heart;
        public static Rect bounds = ApplicationView.GetForCurrentView().VisibleBounds;
        public static float DesignWidth = 1920;
        public static float DesignHeight = 1080;
        public static float scaleWidth, scaleHeight;
        public static float MyScore, boomX, boomY;
        public static int boomCount = 60;
        //To increase aliens, change this and the for loop
        public static int totalEnemies = 5;
        public static bool RoundEnded = false;
        public static float fleetPOS = 10;
        public static float fleetDIR = 2;
        public static int Level = 1;
        public static int Health = 3;
        public static int GameState = 0;

        //High Score
        public static string STRHighScore = "0";

        //Round Timers
        public static DispatcherTimer RoundTimer = new DispatcherTimer();
        public static DispatcherTimer EnemyTimer = new DispatcherTimer();

        //Ship
        public static Ship myShip;

        //Lists (Enemies)
        public static List<float> enemyXPOS = new List<float>();
        public static List<float> enemyYPOS = new List<float>();
        public static List<int> enemySHIP = new List<int>();
        public static List<string> enemyDIR = new List<string>();
        public static List<Alien> alienList = new List<Alien>();
        public static Boolean leftMovement = false;
        public static Boolean rightMovement = false;
        public static Boolean shoot = false;

        public MainPage()
        {

            this.InitializeComponent();
            Window.Current.SizeChanged += Current_SizeChanged;

            Scaling.SetScale();
            RoundTimer.Tick += RoundTimer_Tick;
            RoundTimer.Interval = new TimeSpan(0, 0, 1);

            EnemyTimer.Tick += EnemyTimer_Tick;

            //Creating the high score file
            Storage.CreateFile();
            Storage.ReadFile();

            //64 if half of spaceship.png and 200 is more than 128 to give space below the ship 
            myShip = new Ship((float)bounds.Width / 2 - (64 * scaleWidth), (float)bounds.Height - (200 * scaleHeight));
        }

        private void EnemyTimer_Tick(object sender, object e)
        {
            //Change if totalEnemies changes
            for (int a = 0; a < 5; a++)
            {
                if (totalEnemies > 0)
                {
                    Alien myAlien = new Alien((90 * a * scaleHeight), (50 + scaleHeight), 1);
                    alienList.Add(myAlien);
                }

                totalEnemies -= 1;
            }
        }
        private void RoundTimer_Tick(object sender, object e)
        {
            if (alienList.Count < 1)
            {
                RoundTimer.Stop();
                RoundEnded = true;

                //Resets the enemy number after game over 
                totalEnemies = 5;

                //Increases number of levels
                Level += 1;

                //Decrease health to test score screens
                Health -= 1;
            }
        }

        private void Current_SizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            bounds = ApplicationView.GetForCurrentView().VisibleBounds;
            Scaling.SetScale();
        }

        private void GameCanvas_CreateResources(Microsoft.Graphics.Canvas.UI.Xaml.CanvasControl sender, Microsoft.Graphics.Canvas.UI.CanvasCreateResourcesEventArgs args)
        {
            args.TrackAsyncAction(CreateResourcesAsync(sender).AsAsyncAction());
        }

        async Task CreateResourcesAsync(CanvasControl sender)
        {
            StartScreen = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/startedit.png"));
            Level1 = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/background2edit.png"));
            Continue = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/continueedit.png"));
            GameOver = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/gameoveredit.png"));
            Rules = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/rulesedit.png"));
            Photon = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/beam.png"));
            MyShip = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/spaceship.png"));
            Enemy1 = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/alien.png"));
            Enemy2 = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/alien2.png"));
            Boom = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/boom.png"));
            Heart = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/lifecount.png"));
        }
        private void GameCanvas_Draw(Microsoft.Graphics.Canvas.UI.Xaml.CanvasControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasDrawEventArgs args)
        {
            GSM.gamelevel();
            args.DrawingSession.DrawImage(Scaling.img(BG));
            if (RoundEnded == true)
            {
                //Positions the round score after game
                CanvasTextLayout textLayout1 = new CanvasTextLayout(args.DrawingSession,"Score\n" + MyScore.ToString(), new CanvasTextFormat() { FontSize = (35 * scaleHeight), WordWrapping = CanvasWordWrapping.NoWrap }, 0.0f, 0.0f);
                //Positions the current high score after game
                CanvasTextLayout textLayout2 = new CanvasTextLayout(args.DrawingSession, "High Score\n" + Storage.highScore.ToString(), new CanvasTextFormat() { FontSize = (35 * scaleHeight), WordWrapping = CanvasWordWrapping.NoWrap }, 0.0f, 0.0f);
                args.DrawingSession.DrawTextLayout(textLayout1, ((DesignWidth * scaleWidth) / 2) - ((float)textLayout1.DrawBounds.Width / 2), 400 * scaleHeight, Colors.White);
                args.DrawingSession.DrawTextLayout(textLayout2, ((DesignWidth * scaleWidth) / 2) - ((float)textLayout1.DrawBounds.Width / 2), 560 * scaleHeight, Colors.White);
            }
            else
            {
                if (GameState > 1)
                {
                    //Positions the level number during game
                    args.DrawingSession.DrawText("Level: " + Level.ToString(), (float)bounds.Width / 2 - 440, (float)bounds.Height - 45, Color.FromArgb(255, 255, 255, 255));
                    //Positions the score board during game
                    args.DrawingSession.DrawText("Score: " + MyScore.ToString(), (float)bounds.Width / 2 - 40, (float)bounds.Height - 45, Color.FromArgb(255, 255, 255, 255));
                    myShip.MoveShip();

                    //Displaying life count
                    args.DrawingSession.DrawText("Lifes: " , (float)bounds.Width / 2 + 400, (float)bounds.Height - 45, Color.FromArgb(255, 255, 255, 255));
                    for(int i = 0; i < Health; i++)
                    {
                        args.DrawingSession.DrawImage(Scaling.img(Heart), (float)bounds.Width / 2 + (450 + (60 * i)), (float)bounds.Height - 55);
                    }

                    //Display Enemies
                    for (int j = 0; j < alienList.Count; j++)
                    {
                        if (alienList[j].AlienType == 1)
                        {
                            ALIEN_IMG = Enemy1;
                        }
                        if (alienList[j].AlienType == 2)
                        {
                            ALIEN_IMG = Enemy2;
                        }

                        alienList[j].MoveAlien();
                        //Alien.png needs no dimension scaling
                        args.DrawingSession.DrawImage(Scaling.img(ALIEN_IMG), alienList[j].AlienXPOS, alienList[j].AlienYPOS);
                    }

                    //Display explosion
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

                    //Display Projectiles
                    for (int i = 0; i < myShip.getBulletX().Count; i++)
                    {
                        //Beam.png needs no dimension scaling
                        args.DrawingSession.DrawImage(Scaling.img(Photon), myShip.getBulletX()[i], myShip.getBulletY()[i]);

                        for (int h = 0; h < alienList.Count; h++)
                        {
                            //100 and 91 are dimensions from boom.png
                            if (myShip.getBulletX()[i] >= alienList[h].AlienXPOS && myShip.getBulletX()[i] <= alienList[h].AlienXPOS + (100 * scaleWidth) && myShip.getBulletY()[i] >= alienList[h].AlienYPOS && myShip.getBulletY()[i] <= alienList[h].AlienYPOS + (91 * scaleHeight))
                            {
                                //50 is half of boom.png width 100 and 91 is also from boom.png
                                boomX = myShip.getBulletX()[i] - (50 * scaleWidth);
                                boomY = myShip.getBulletY()[i] - (91 * scaleHeight);

                                MyScore = MyScore + alienList[h].AlienScore;

                                alienList.RemoveAt(h);
                                myShip.removeBullet(i);

                                break;
                            }
                        }

                    }
                    args.DrawingSession.DrawImage(Scaling.img(MyShip), myShip.ShipXPOS, myShip.ShipYPOS);
                }
            }
            GameCanvas.Invalidate();
        }

        private void GameCanvas_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (RoundEnded == true)
                {
                //Button pixel positions on the gameoveredit.png for return to start
                if (((float)e.GetPosition(GameCanvas).X > 621 * scaleWidth && (float)e.GetPosition(GameCanvas).X < 1303 * scaleWidth) && (float)e.GetPosition(GameCanvas).Y > 946 * scaleHeight && (float)e.GetPosition(GameCanvas).Y < 1008 * scaleHeight)
                {
                    RoundEnded = false;

                    //Stop Enemy Timer
                    EnemyTimer.Stop();
                    enemyXPOS.Clear();
                    enemyYPOS.Clear();
                    enemySHIP.Clear();
                    enemyDIR.Clear();
                    MyScore = 0;
                    GameState = 0;

                    //Resets health
                    Health = 3;

                    //Resets level count
                    Level = 1;
                }
                //Button pixel positions on the gameoveredit.png for continue
                if (((float)e.GetPosition(GameCanvas).X > 621 * scaleWidth && (float)e.GetPosition(GameCanvas).X < 1303 * scaleWidth) && (float)e.GetPosition(GameCanvas).Y > 826 * scaleHeight && (float)e.GetPosition(GameCanvas).Y < 891 * scaleHeight)
                {
                    RoundEnded = false;
                }
            }
            else{
                if (GameState != 2)
                {
                    //Button pixel positions on the startedit.png for the how to play button
                    if (((float)e.GetPosition(GameCanvas).X > 768 * scaleWidth && (float)e.GetPosition(GameCanvas).X < 1152 * scaleWidth) && (float)e.GetPosition(GameCanvas).Y > 723 * scaleHeight && (float)e.GetPosition(GameCanvas).Y < 831 * scaleHeight)
                    {
                        GameState = 1;
                    }

                    //Button pixel positions on the rulesedit.png
                    if (((float)e.GetPosition(GameCanvas).X > 1417 * scaleWidth && (float)e.GetPosition(GameCanvas).X < 1836 * scaleWidth) && (float)e.GetPosition(GameCanvas).Y > 907 * scaleHeight && (float)e.GetPosition(GameCanvas).Y < 1015 * scaleHeight)
                    {
                        GameState = 0;
                    }

                    //Button pixel positions on the startedit.png for the start game button
                    if (((float)e.GetPosition(GameCanvas).X > 270 * scaleWidth && (float)e.GetPosition(GameCanvas).X < 656 * scaleWidth) && (float)e.GetPosition(GameCanvas).Y > 479 * scaleHeight && (float)e.GetPosition(GameCanvas).Y < 589 * scaleHeight)
                    {
                        GameState = 2;
                        RoundTimer.Start();
                        EnemyTimer.Start();
                        Ship.bulletTimer.Start();
                    }
                }
                else if (GameState > 1) { }
            }
        }
    }
}
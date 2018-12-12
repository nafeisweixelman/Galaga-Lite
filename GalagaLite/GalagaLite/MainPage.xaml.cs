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
        public static CanvasBitmap BG, Rules, StartScreen, Level1, ScoreScreen, Photon, Enemy1, Enemy2, ALIEN_IMG, MyShip, Boom;
        public static Rect bounds = ApplicationView.GetForCurrentView().VisibleBounds;
        public static float DesignWidth = 1920;
        public static float DesignHeight = 1080;
        public static float scaleWidth, scaleHeight;
        public static float MyScore, boomX, boomY;
        public static int boomCount = 60;
        public static int totalEnemies = 5;
        public static bool RoundEnded = false;
        public static float fleetPOS = 10;
        public static float fleetDIR = 2;
        public static int Level = 1;

        public static int GameState = 0;
        //High Score
        public static string STRHighScore = "0";

        public static DispatcherTimer RoundTimer = new DispatcherTimer();
        public static DispatcherTimer EnemyTimer = new DispatcherTimer();

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

            //64 if half of spaceship.png and 150 is more than 128 to give space below the ship
            myShip = new Ship((float)bounds.Width / 2 - (64 * scaleWidth), (float)bounds.Height - (150 * scaleHeight));
        }


        private void EnemyTimer_Tick(object sender, object e)
        {
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
            StartScreen = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/galaga_logo.png"));
            Level1 = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/background2.png"));
            ScoreScreen = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/scorescreen.png"));
            Rules = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/rules.png"));
            Photon = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/beam.png"));
            MyShip = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/spaceship.png"));
            Enemy1 = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/alien.png"));
            Enemy2 = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/alien2.png"));
            Boom = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/boom.png"));
        }
        private void GameCanvas_Draw(Microsoft.Graphics.Canvas.UI.Xaml.CanvasControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasDrawEventArgs args)
        {
            GSM.gamelevel();
            args.DrawingSession.DrawImage(Scaling.img(BG));
            if (RoundEnded == true)
            {
                Storage.UpdateScore();

                CanvasTextLayout textLayout1 = new CanvasTextLayout(args.DrawingSession, MyScore.ToString(), new CanvasTextFormat() { FontSize = (36 * scaleHeight), WordWrapping = CanvasWordWrapping.NoWrap }, 0.0f, 0.0f);
                //Positions the highscore board after game
                args.DrawingSession.DrawTextLayout(textLayout1, ((DesignWidth * scaleWidth) / 2) - ((float)textLayout1.DrawBounds.Width / 2 - 20), 820 * scaleHeight, Colors.White);
                args.DrawingSession.DrawText("HighScores\n" + Convert.ToInt16(STRHighScore), new Vector2(200, 200), Color.FromArgb(255, 200, 150, 210));
            }
            else
            {
                if (GameState > 1)
                {
                    //Positions the level number during game
                    args.DrawingSession.DrawText("Level: " + Level.ToString(), (float)bounds.Width / 2, 10, Color.FromArgb(255, 255, 255, 255));
                    // Positions the score board during game
                    args.DrawingSession.DrawText("Score: " + MyScore.ToString(), (float)bounds.Width / 2, 40, Color.FromArgb(255, 255, 255, 255));
                    // Positions the highscore board during game
                    args.DrawingSession.DrawText("High Score: " + Convert.ToInt16(STRHighScore), (float)bounds.Width / 2, 100, Color.FromArgb(255, 255, 255, 255));
                    myShip.MoveShip();

                    //Enemies
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
                    //draws explosion BOOM
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
                //Button pixel positions on the scorescreen.png
                if (((float)e.GetPosition(GameCanvas).X > 795 * scaleWidth && (float)e.GetPosition(GameCanvas).X < 1251 * scaleWidth) && (float)e.GetPosition(GameCanvas).Y > 867 * scaleHeight && (float)e.GetPosition(GameCanvas).Y < 987 * scaleHeight)
                {
                    GameState = 0;
                    RoundEnded = false;

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
                    //Button pixel positions on the galaga_logo.png for the how to play button
                    if (((float)e.GetPosition(GameCanvas).X > 1102 * scaleWidth && (float)e.GetPosition(GameCanvas).X < 1383 * scaleWidth) && (float)e.GetPosition(GameCanvas).Y > 803 * scaleHeight && (float)e.GetPosition(GameCanvas).Y < 870 * scaleHeight)
                    {
                        GameState = 1;
                    }

                    //Button pixel positions on the rules.png
                    if (((float)e.GetPosition(GameCanvas).X > 258 * scaleWidth && (float)e.GetPosition(GameCanvas).X < 624 * scaleWidth) && (float)e.GetPosition(GameCanvas).Y > 670 * scaleHeight && (float)e.GetPosition(GameCanvas).Y < 805 * scaleHeight)
                    {
                        GameState = 0;
                    }

                    //Button pixel positions on the galaga_logo.png for the start game button
                    if (((float)e.GetPosition(GameCanvas).X > 546 * scaleWidth && (float)e.GetPosition(GameCanvas).X < 826 * scaleWidth) && (float)e.GetPosition(GameCanvas).Y > 799 * scaleHeight && (float)e.GetPosition(GameCanvas).Y < 865 * scaleHeight)
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
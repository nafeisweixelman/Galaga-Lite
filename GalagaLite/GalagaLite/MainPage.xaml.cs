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

namespace GalagaLite
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static CanvasBitmap BG, StartScreen, Level1, ScoreScreen;
        public static Rect bounds = ApplicationView.GetForCurrentView().VisibleBounds;
        public static float DesignWidth = 1920;
        public static float DesignHeight = 1080;
        public static float scaleWidth, scaleHeight;
        public static int countdown = 10;
        public static bool RoundEnded = false;

        public static int GameState = 0;             //0 refers to StartScreen, 1 to Level1
        public static DispatcherTimer RoundTimer = new DispatcherTimer();

        public MainPage()
        {
            this.InitializeComponent();
            Window.Current.SizeChanged += Current_SizeChanged;
            Scaling.SetScale();

            RoundTimer.Tick += RoundTimer_Tick;
            RoundTimer.Interval = new TimeSpan(0, 0, 1);       //hours, minutes, seconds
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
        }

        private void GameCanvas_Draw(Microsoft.Graphics.Canvas.UI.Xaml.CanvasControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasDrawEventArgs args)
        {
            gsm.GSM();
            args.DrawingSession.DrawImage(Scaling.img(BG));
            args.DrawingSession.DrawText(countdown.ToString(), 120, 120, Colors.White);
            GameCanvas.Invalidate();
        }

        private void GameCanvas_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            if (RoundEnded == true)
            {
                GameState = 0;
            }
            else
            {
                if (GameState == 0)
                {
                    GameState += 1;
                    RoundTimer.Start();
                }
            }
        }
    }
}

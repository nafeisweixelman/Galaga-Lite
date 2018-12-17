using System;
using System.Collections.Generic;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace GalagaLite.Class
{
    public class Ship
    {
        public Boolean leftMovement;
        public Boolean rightMovement;
        public Boolean shoot;
        public static DispatcherTimer bulletTimer = new DispatcherTimer();

        public float ShipXPOS { set; get; }
        public float ShipYPOS { set; get; }
        public int shootWait = 0;
        public List<float> BulletXPOS = new List<float>();
        public List<float> BulletYPOS = new List<float>();

        /// <summary>
        /// Default constructor for ship class
        /// </summary>
        public Ship()
        {
            ShipXPOS = 0;
            ShipYPOS = 0;
            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
            Window.Current.CoreWindow.KeyUp += CoreWindow_KeyUp;
            bulletTimer.Tick += bulletTimer_Tick;
            bulletTimer.Interval = new TimeSpan(0, 0, 0, 0, 50);
        }


        /// <summary>
        /// Constructor for Ship Class
        /// </summary>
        /// <param name="XPOS"></param>
        /// <param name="YPOS"></param>
        public Ship(float XPOS, float YPOS)
        {
            ShipXPOS = XPOS;
            ShipYPOS = YPOS;
            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
            Window.Current.CoreWindow.KeyUp += CoreWindow_KeyUp;
            bulletTimer.Tick += bulletTimer_Tick;
            bulletTimer.Interval = new TimeSpan(0, 0, 0, 0, 0);
        }
        /// <summary>
        /// shootWait int variable deincremented to limit amount of shots per second
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bulletTimer_Tick(object sender, object e)
        {
            if (shootWait > 0)
                shootWait--;
        }
        /// <summary>
        /// Returns bulletXPOS list
        /// </summary>
        /// <returns></returns>
        public List<float> getBulletX()
        {
            return BulletXPOS;
        }
        /// <summary>
        /// returns bulletYPOS list
        /// </summary>
        /// <returns></returns>
        public List<float> getBulletY()
        {
            return BulletYPOS;
        }
        /// <summary>
        /// removes a bullet by removing its X and Y coordinates
        /// </summary>
        /// <param name="num"></param>
        public void removeBullet(int num)
        {
            BulletXPOS.RemoveAt(num);
            BulletYPOS.RemoveAt(num);
        }
        /// <summary>
        /// CoreWindow_KeyDown checks to see if a key is being held down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void CoreWindow_KeyDown(CoreWindow sender, KeyEventArgs args)
        {
            if (args.VirtualKey == VirtualKey.A)
            {
                leftMovement = true;
            }
            if (args.VirtualKey == VirtualKey.D)
            {
                rightMovement = true;
            }
            if (args.VirtualKey == VirtualKey.Space)
            {
                shoot = true;
                if (shootWait == 0)
                {
                    //adding bullet at top center of space ship
                    BulletXPOS.Add(ShipXPOS + (46 * MainPage.scaleWidth));
                    BulletYPOS.Add(ShipYPOS);

                    shootWait = 10; //limiting firing to half a second
                }
            }
        }
        /// <summary>
        /// CoreWindow_KeyUp checks to see if A, D, or Space key are no longer being held
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void CoreWindow_KeyUp(CoreWindow sender, KeyEventArgs args)
        {
            if (args.VirtualKey == VirtualKey.A)
            {
                leftMovement = false;
            }
            if (args.VirtualKey == VirtualKey.D)
            {
                rightMovement = false;
            }
            if (args.VirtualKey == VirtualKey.Space)
            {
                shoot = false;
            }
        }
        /// <summary>
        /// moves ship within bounds of screen left or right
        /// moves bullet directly upwards and removes once off screen
        /// </summary>
        public void MoveShip()
        {
            if (rightMovement && ShipXPOS < (float)MainPage.bounds.Width - (93 * MainPage.scaleWidth))
            {
                ShipXPOS += 6;
            }
            if (leftMovement && ShipXPOS > 0f)
            {
                ShipXPOS -= 6;
            }
            for (int a = 0; a < BulletXPOS.Count; a++)
            {
                BulletYPOS[a] -= 10;
                if (BulletYPOS[a] < 0f)
                {
                    removeBullet(a);
                }
            }
        }

    }
}
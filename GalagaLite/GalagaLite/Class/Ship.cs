using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public Ship()
        {
            ShipXPOS = 0;
            ShipYPOS = 0;
            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
            Window.Current.CoreWindow.KeyUp += CoreWindow_KeyUp;
            bulletTimer.Tick += bulletTimer_Tick;
            bulletTimer.Interval = new TimeSpan(0, 0, 0, 0, 50);
        }



        public Ship(float XPOS, float YPOS)
        {
            ShipXPOS = XPOS;
            ShipYPOS = YPOS;
            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
            Window.Current.CoreWindow.KeyUp += CoreWindow_KeyUp;
            bulletTimer.Tick += bulletTimer_Tick;
            bulletTimer.Interval = new TimeSpan(0, 0, 0 , 0, 50);
        }
        private void bulletTimer_Tick(object sender, object e)
        {
            if(shootWait > 0)
                shootWait--;
        }
        public List<float> getBulletX()
        {
            return BulletXPOS;
        }
        public List<float> getBulletY()
        {
            return BulletYPOS;
        }
        public void removeBullet(int num)
        {
            BulletXPOS.RemoveAt(num);
            BulletYPOS.RemoveAt(num);
        }
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
            }
        }
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
                if (shootWait == 0)
                {
                    BulletXPOS.Add(ShipXPOS + (46 * MainPage.scaleWidth));
                    BulletYPOS.Add(ShipYPOS);
                    shootWait = 10;
                }
            }
        }

        public void MoveShip()
        {
            if (rightMovement && ShipXPOS < (float)MainPage.bounds.Width - (93 * MainPage.scaleWidth))
            {
                ShipXPOS += 3;
            }
            if (leftMovement && ShipXPOS > 0f)
            {
                ShipXPOS -= 3;
            }
            for (int a = 0; a < BulletXPOS.Count; a++)
            {
                BulletYPOS[a] -= 10;
                if(BulletYPOS[a] < 0f)
                {
                    removeBullet(a);
                }
            }
        }

    }
}

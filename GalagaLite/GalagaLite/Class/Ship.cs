using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Core;

namespace GalagaLite.Class
{
    public class Ship
    {
        public static Boolean leftMovement;
        public static Boolean rightMovement;
        public static Boolean shoot;
        public float ShipXPOS { set; get; }
        public float ShipYPOS { set; get; }

        public Ship()
        {
            ShipXPOS = 0;
            ShipYPOS = 0;
        }
        public Ship(float XPOS, float YPOS)
        {
            ShipXPOS = XPOS;
            ShipYPOS = YPOS;
        }
        public static void CoreWindow_KeyDown(CoreWindow sender, KeyEventArgs args)
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
        public static void CoreWindow_KeyUp(CoreWindow sender, KeyEventArgs args)
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

        public void MoveShip()
        {
            if (rightMovement)
            {
                ShipXPOS += 3;
            }
            if (leftMovement)
            {
                ShipXPOS -= 3;
            }
        }
    }
}

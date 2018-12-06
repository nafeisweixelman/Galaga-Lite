using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Core;

namespace GalagaLite.Class
{
    class SpaceShip
    {
        private void move(object sender, KeyEventArgs e)
        {
            if(e.VirtualKey == VirtualKey.A)
            {
                MainPage.ShipXPOS -= 3;
            }
            if(e.VirtualKey == VirtualKey.D)
            {
                MainPage.ShipXPOS += 3;
            }
        }
    }
}

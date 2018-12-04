using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galaga.Class
{
    class GSM
    {
        public static void Gsm()
        {
            if (MainPage.roundEnded == true)
                MainPage.backGround = MainPage.scoreScreen;
            else if (MainPage.gameState == 0)
                MainPage.backGround = MainPage.startScreen;
            else if (MainPage.gameState == 1)
                MainPage.backGround = MainPage.level1;
        }
            
    }
}

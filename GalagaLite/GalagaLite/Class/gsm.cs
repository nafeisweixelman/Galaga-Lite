using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalagaLite.Class
{
    class GSM
    {
        public static void gamelevel()
        {
            if (MainPage.GameState == 0)
            {
                MainPage.BG = MainPage.StartScreen;
            }
            else if (MainPage.GameState == 1)
            {
                MainPage.BG = MainPage.Rules;
            }
            else
            {
                MainPage.BG = MainPage.Level1;
            }

        }

    }
}

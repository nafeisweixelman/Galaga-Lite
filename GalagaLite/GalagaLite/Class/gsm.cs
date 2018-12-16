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
            if ((MainPage.RoundEnded == true) && (MainPage.Health == 0))
            {
                MainPage.BG = MainPage.GameOver;
            }
            else if (MainPage.RoundEnded == true)
            {
                MainPage.BG = MainPage.Continue;
            }
            else
            {
                if (MainPage.GameState == 0)
                {
                    MainPage.BG = MainPage.StartScreen;
                }
                else if (MainPage.GameState == 1)
                {
                    MainPage.BG = MainPage.Rules;
                }
                /*else if (MainPage.GameState == 2)
                {
                    MainPage.BG = MainPage.Continue;
                }*/
                else if (MainPage.GameState == 2)
                {
                    MainPage.BG = MainPage.Level1;
                }
            }

        }

    }
}

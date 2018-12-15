using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalagaLite.Class
{
    class GSM
    {
        public static int level = 1;

        public static void gameLevel()
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

        public static void nextLevel()
        {
            level++;
            MainPage.GameState = 2;
            MainPage.RoundEnded = false;


            MainPage.EnemyTimer.Stop();
            MainPage.enemyXPOS.Clear();
            MainPage.enemyYPOS.Clear();
            MainPage.enemySHIP.Clear();
            MainPage.enemyDIR.Clear();
        }

        public static void startGame()
        {
            MainPage.RoundTimer.Start();
            MainPage.EnemyTimer.Start();
            Ship.bulletTimer.Start();
        }

        public static void endGame()
        {
            MainPage.GameState = 0;
            MainPage.RoundEnded = false;

            //Stop Enemy Timer
            MainPage.EnemyTimer.Stop();
            MainPage.enemyXPOS.Clear();
            MainPage.enemyYPOS.Clear();
            MainPage.enemySHIP.Clear();
            MainPage.enemyDIR.Clear();
            MainPage.MyScore = 0;
        }

    }
}

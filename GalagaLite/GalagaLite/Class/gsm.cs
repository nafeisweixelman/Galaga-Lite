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
        public static int totalEnemies = 3, holdEnemies = totalEnemies;

        public static void gameLevel()
        {
            if (MainPage.RoundEnded == true && MainPage.lives == 0)
            {
                MainPage.BG = MainPage.GameOver;
            }
            else if (MainPage.RoundEnded == true && MainPage.lives > 0)
                MainPage.BG = MainPage.Continue;
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
                else
                {
                    MainPage.BG = MainPage.Level1;
                }
            }
            

        }

        public static void nextLevel()
        {
            level++;
            MainPage.GameState = 2;
            MainPage.RoundEnded = false;
            if(level <= 5)
                holdEnemies += 2;
            totalEnemies = holdEnemies;

            MainPage.alienList.Clear();
            MainPage.RoundTimer.Stop();
            MainPage.EnemyTimer.Stop();

            startGame();
        }

        public static void startGame()
        {
            MainPage.RoundTimer.Start();
            MainPage.EnemyTimer.Start();
            Ship.bulletTimer.Start();
        }

        public static void pauseGame()
        {
            if (MainPage.count == 2)
                nextLevel();
            else if (MainPage.count == 0)
                endGame();
        }

        public static void endGame()
        {
            MainPage.GameState = 0;
            MainPage.RoundEnded = false;
            MainPage.lives = 1;
            level = 1;
            MainPage.count = 1;

            holdEnemies = 3;
            totalEnemies = holdEnemies;

            MainPage.alienList.Clear();
            MainPage.RoundTimer.Stop();
            //Stop Enemy Timer
            MainPage.EnemyTimer.Stop();
            MainPage.MyScore = 0;
        }

    }
}

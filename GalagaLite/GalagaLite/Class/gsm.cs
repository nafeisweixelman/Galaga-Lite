namespace GalagaLite.Class
{
    class gsm
    {
        public static void GSM()
        {
            if (MainPage.RoundEnded == true)
            {
                MainPage.BG = MainPage.ScoreScreen;
            }
            else
            {
                if (MainPage.GameState == 0)
                {
                    MainPage.BG = MainPage.StartScreen;
                }
                else if (MainPage.GameState == 1)
                {
                    MainPage.BG = MainPage.Level1;
                }
            }
        }
    }
}

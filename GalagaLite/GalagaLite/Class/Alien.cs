namespace GalagaLite.Class
{
    public class Alien
    {
        public float AlienXPOS { get; set; }
        public float AlienYPOS { get; set; }
        public float SetYPOS;
        public int AlienScore { get; set; }
        public int AlienType { get; }
        public static int fleetPOS = 10;
        public static int fleetDIR = 1;
        /// <summary>
        /// Default constructor for alien class
        /// </summary>
        public Alien()
        {
            AlienXPOS = 0;
            AlienYPOS = 0;
        }
        public static void createAliens()
        {
            for (int a = 0; a < GSM.holdEnemies; a++)
            {
                if (GSM.totalEnemies > 0)
                {
                    Alien myAlien = new Alien((90 * a * MainPage.scaleHeight), (50 + MainPage.scaleHeight), 1);
                    MainPage.alienList.Add(myAlien);
                }

                GSM.totalEnemies -= 1;
            }
        }
        /// <summary>
        /// Constructor for alien class
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="type"></param>
        public Alien(float x, float y, int type)
        {
            AlienXPOS = x;
            AlienYPOS = y - 100;
            SetYPOS = y;
            AlienType = type;
            switch (type)
            {
                case 1:
                    AlienScore = 100;
                    break;
                case 2:
                    AlienScore = 150;
                    break;
                default:
                    AlienScore = 0;
                    break;
            }

        }
        /// <summary>
        /// Move function for alien class to prevent any aliens moving off the screen
        /// </summary>

        public void MoveAlien()
        {
            if (MainPage.alienList[(MainPage.alienList.Count) - 1].AlienXPOS > (MainPage.bounds.Width - 70 * MainPage.scaleWidth))
            {
                fleetDIR = -1;
            }
            if (MainPage.alienList[0].AlienXPOS < 10)
            {
                fleetDIR = 1;
            }
            AlienXPOS += fleetDIR;
            if (AlienYPOS >= SetYPOS + 5 || AlienYPOS <= SetYPOS - 5)
            {
                AlienYPOS += 2;
            }
            else
            {
                AlienYPOS = SetYPOS;
            }
            if (AlienYPOS > MainPage.bounds.Height)
            {
                AlienYPOS = -40;
            }
        }



    }
}
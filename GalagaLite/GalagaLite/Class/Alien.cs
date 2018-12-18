namespace GalagaLite.Class
{
    public class Alien
    {
        public float AlienXPOS { get; set; }
        public float AlienYPOS { get; set; }
        public float SetYPOS;
        public float SetXPOS;
        public float shootXPOS;
        public float shootYPOS;
        public int AlienScore { get; set; }
        public int AlienType { get; }
        public static int fleetPOS = 1;
        public static int fleetDIRR = 1;
        public static int fleetDIRL = -1;
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
            SetXPOS = x;
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
            AlienXPOS += fleetDIR;
            if (AlienYPOS <= SetYPOS - 5)
            {
                AlienYPOS += 2;
            }
            else if(AlienYPOS >= SetYPOS + 5)
            {
                if(AlienXPOS < 0)
                {
                    AlienXPOS += 5;
                }
                AlienYPOS += 2;
                AlienXPOS += fleetDIR * 5;
            }
            else
            {
                AlienYPOS = SetYPOS;
            }
            if (AlienYPOS > MainPage.bounds.Height)
            {
                AlienYPOS = -40;
                AlienXPOS = SetXPOS + fleetPOS;
            }
            if(AlienYPOS < MainPage.bounds.Height)
            {
                attack();
            }
        }
        public void MoveFleet()
        {
            fleetPOS += fleetDIR;
            if(fleetPOS > 40)
            {
                fleetDIR = fleetDIRL;
            }
            if(fleetPOS < 1)
            {
                fleetDIR = fleetDIRR;
            }
        }

        public void attack()
        {
            shootXPOS = AlienXPOS;
            shootYPOS = AlienYPOS;
        }



    }
}
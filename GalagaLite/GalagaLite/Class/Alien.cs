using System;
using System.Collections.Generic;

namespace GalagaLite.Class
{
    public class Alien
    {
        public float AlienXPOS { get; set; }
        public float AlienYPOS { get; set; }
        public static float alienDown = 4;
        public float SetYPOS;
        public float SetXPOS;
        public List<float> shootXPOS = new List<float>();
        public List<float> shootYPOS = new List<float>();
        public int AlienScore { get; set; }
        public int AlienType { get; }
        public static int fleetPOS = 1;
        public static int fleetDIRR = 1;
        public static int fleetDIRL = -1;
        public static int fleetDIR = 1;
        public static float xpos, xpos2;
        public static int hold, hold2;

        public Boolean attacked = false;
        
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
                    xpos = (float)(MainPage.bounds.Width / 2) -(GSM.totalEnemies * 45) + (a * 45 * MainPage.scaleHeight);
                    Alien myAlien = new Alien(xpos, (50 + MainPage.scaleHeight), 2);
                    MainPage.alienList.Add(myAlien);
                    hold++;

                    xpos2 = (float)(MainPage.bounds.Width / 2) - (GSM.totalEnemies * 45) + (a * 45 * MainPage.scaleHeight);
                    Alien myAlien2 = new Alien(xpos2, (150 + MainPage.scaleHeight), 1);
                    MainPage.alienList.Add(myAlien2);
                    hold2++;
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
                AlienYPOS += alienDown;
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
                attacked = false;
            }
            if(AlienYPOS > MainPage.bounds.Height / 4 && !attacked)
            {
                attack();
                attacked = true;
            }
            for(int a = 0; a < shootXPOS.Count; a++)
            {
                shootYPOS[a] += 5;
                if(shootYPOS[a] > MainPage.bounds.Height)
                {
                    shootXPOS.RemoveAt(a);
                    shootYPOS.RemoveAt(a);
                }
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
            shootXPOS.Add(AlienXPOS + (35 * MainPage.scaleWidth));
            shootYPOS.Add(AlienYPOS);
        }

        public List<float> getShootX()
        {
            return shootXPOS;
        }
        public List<float> getShootY()
        {
            return shootYPOS;
        }

        public void removeShoot(int a)
        {
            shootXPOS.RemoveAt(a);
            shootYPOS.RemoveAt(a);
        }



    }
}
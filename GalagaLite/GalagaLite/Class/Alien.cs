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

        public Boolean attacked = false;
        /// <summary>
        /// Default constructor for alien class
        /// </summary>
        public Alien()
        {
            AlienXPOS = 0;
            AlienYPOS = 0;
        }
        /// <summary>
        /// function to create aliens
        /// </summary>
        public static void createAliens()
        {
            for (int a = 0; a < GSM.holdEnemies; a++)
            {
                if (GSM.totalEnemies > 0)
                {
                    Alien myAlien = new Alien((float)(MainPage.bounds.Width / 2) - (GSM.totalEnemies * 45) + (a * 45 * MainPage.scaleHeight), (50 + MainPage.scaleHeight), 1); 
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
            AlienXPOS += fleetDIR; //moves alien in direction of fleet

            //if alien is above the set position then moves alien down into it
            if (AlienYPOS <= SetYPOS - 5)
            {
                AlienYPOS += 2;
            }
            //if alien is below set position then moves down and sets it in attack
            else if(AlienYPOS >= SetYPOS + 5)
            {
                if(AlienXPOS < 0)
                {
                    AlienXPOS += 5;
                }
                AlienYPOS += alienDown; //alien moves down at variable speed set by level
                AlienXPOS += fleetDIR * 5; // alien moves side to side more when attacking
            }
            //if alien is in bounds it goes to setYPOS
            else
            {
                AlienYPOS = SetYPOS;
            }
            //if alien goes off screen at bottom it resets up top
            if (AlienYPOS > MainPage.bounds.Height)
            {
                AlienYPOS = -40;
                AlienXPOS = SetXPOS + fleetPOS;
                attacked = false;
            }
            //if alien is quarter way down screen then it shoots laser
            if(AlienYPOS > MainPage.bounds.Height / 4 && !attacked)
            {
                attack();
                attacked = true;
            }
            //moving alien projectile and deleting it off screen
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
        /// <summary>
        /// moves fleet between 0 and 41
        /// </summary>
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
        /// <summary>
        /// returns shootXPOS list
        /// </summary>`
        /// <returns></returns>
        public List<float> getShootX()
        {
            return shootXPOS;
        }
        public List<float> getShootY()
        {
            return shootYPOS;
        }
        /// <summary>
        /// removes alien laser at 
        /// </summary>
        /// <param name="a"></param>
        public void removeShoot(int a)
        {
            shootXPOS.RemoveAt(a);
            shootYPOS.RemoveAt(a);
        }



    }
}
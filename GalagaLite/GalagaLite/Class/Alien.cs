﻿namespace GalagaLite.Class
{
    public class Alien
    {
        public float AlienXPOS { get; set; }
        public float AlienYPOS { get; set; }
        public int AlienScore { get; set; }
        public int AlienType { get; }
        public static int fleetPOSU = 3;
        public static int fleetPOSD = -3;
        public static int fleetDIRR = 3;
        public static int fleetDIRL = -3;
        public static int fleetDIR  = 3;
        public static int fleetPOS = 3;
        /// <summary>
        /// Default constructor for alien class
        /// </summary>
        public Alien()
        {
            AlienXPOS = 0;
            AlienYPOS = 0;
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
            AlienYPOS = y;
            AlienType = type;
            switch (type)
            {
                case 1:
                    AlienScore = 1000;
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
                fleetDIR = fleetDIRL;
            }
            if (MainPage.alienList[0].AlienXPOS < 10)
            {
                fleetDIR = fleetDIRR;
            }

            AlienXPOS += fleetDIR;

            if (MainPage.alienList[(MainPage.alienList.Count) - 1].AlienYPOS > (MainPage.bounds.Height - 70 * MainPage.scaleHeight))
                fleetPOS = fleetPOSD;
            if (MainPage.alienList[0].AlienYPOS < 10)
               fleetPOS = fleetPOSU;
                
            AlienYPOS += fleetPOS;
        }
    }
}
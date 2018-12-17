using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalagaLite.Class
{
    public class Alien
    {
        public float AlienXPOS { get; set; }
        public float AlienYPOS { get; set; }
        public int AlienScore { get; set; }
        public int AlienType { get; }
        public static int fleetPOSU = 1;
        public static int fleetPOSD = -1;
        public static int fleetDIRR = 1;
        public static int fleetDIRL = -1;
        public static int fleetDIR  = 1;
        public static int fleetPOS = 1;
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
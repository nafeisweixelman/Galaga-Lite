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
        public static int fleetPOS = 10;
        public static int fleetDIR = 1;

        public Alien()
        {
            AlienXPOS = 0;
            AlienYPOS = 0;
        }
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
        public void Move()
        {
            if (fleetPOS > 4500)
            {
                fleetDIR = -1;
            }
            if (fleetPOS < 10)
            {
                fleetDIR = 1;
            }
            fleetPOS += fleetDIR;
            AlienXPOS += fleetDIR;
        }


        
    }
}

using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using System.Numerics;

namespace GalagaLite.Class
{
    class Scaling
    {
        public static void SetScale()
        {
            //Display Information
            MainPage.scaleWidth = (float)MainPage.bounds.Width / MainPage.DesignWidth;
            MainPage.scaleHeight = (float)MainPage.bounds.Height / MainPage.DesignHeight;
        }

        public static Transform2DEffect img(CanvasBitmap source)
        {
            Transform2DEffect image;
            image = new Transform2DEffect() { Source = source };
            image.TransformMatrix = Matrix3x2.CreateScale(MainPage.scaleWidth, MainPage.scaleHeight);
            return image;
        }
    }
}

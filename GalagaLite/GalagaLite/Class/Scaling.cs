using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using System.Numerics;

namespace GalagaLite.Class
{
    class Scaling
    {
        /// <summary>
        /// This method sets the scale for us to use for the rest of the game
        /// </summary>
        public static void SetScale()
        {
            MainPage.scaleWidth = (float)MainPage.bounds.Width / MainPage.DesignWidth;
            MainPage.scaleHeight = (float)MainPage.bounds.Height / MainPage.DesignHeight;
        }

        /// <summary>
        /// This allows us to scale the images to what we want
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Transform2DEffect img(CanvasBitmap source)
        {
            Transform2DEffect image;
            image = new Transform2DEffect() { Source = source };
            image.TransformMatrix = Matrix3x2.CreateScale(MainPage.scaleWidth, MainPage.scaleHeight);
            return image;
        }
    }
}

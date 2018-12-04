using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Galaga_Test.Class
{
    class Storage
    {
        public static StorageFolder storage = ApplicationData.Current.LocalFolder;

        public static async void CreateFile()
        {
            try
            {
                await storage.CreateFileAsync("Galaga.txt", CreationCollisionOption.OpenIfExists);
                StorageFile dataFile = await storage.GetFileAsync("Galaga.txt");
                await FileIO.WriteTextAsync(dataFile, "0");
            }
            catch
            {
            }
        }

        public static async void ReadFile()
        {
            try
            {
                StorageFile dataFile = await storage.GetFileAsync("Galaga.txt");
                MainPage.strHighScore = await FileIO.ReadTextAsync(dataFile);
            }
            catch
            {

            }
        }

        public static async void UpdateScore()
        {
            int highScore = Convert.ToInt16(MainPage.strHighScore);
            if (MainPage.myScore > highScore)
            {
                try
                {
                    StorageFile dataFile = await storage.GetFileAsync("Galaga.txt");
                    await FileIO.WriteTextAsync(dataFile, MainPage.myScore.ToString());
                    ReadFile();
                }
                catch
                {

                }
            }
        }
    }
}

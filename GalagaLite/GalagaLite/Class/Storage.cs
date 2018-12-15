using System;
using Windows.Storage;

namespace GalagaLite.Class
{
    class Storage
    {
        public static string filename = "GalagaLiteHighScore.txt";
        public static StorageFolder StorageFolder = ApplicationData.Current.LocalFolder;

        public static async void CreateFile()
        {
            try
            {
                await StorageFolder.CreateFileAsync(filename, CreationCollisionOption.OpenIfExists);
                StorageFile DataFile = await StorageFolder.GetFileAsync(filename);
            }
            catch { }
        }

        public static async void ReadFile()
        {
            try
            {
                StorageFile DataFile = await StorageFolder.GetFileAsync(filename);
                MainPage.STRHighScore = await FileIO.ReadTextAsync(DataFile);
            }
            catch { }
        }

        public static async void UpdateScore()
        {
            int highScore = Convert.ToInt16(MainPage.STRHighScore);
            if (MainPage.MyScore > highScore)
            {
                try
                {
                    StorageFile dataFile = await StorageFolder.GetFileAsync("Galaga.txt");
                    await FileIO.WriteTextAsync(dataFile, MainPage.MyScore.ToString());
                    ReadFile();
                }
                catch
                {

                }
            }

        }
    }
}

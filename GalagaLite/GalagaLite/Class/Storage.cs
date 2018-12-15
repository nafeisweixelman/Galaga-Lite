using System;
using Windows.Storage;

namespace GalagaLite.Class
{
    class Storage
    {
        //High Score Saving location
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
                MainPage.STRHighScore1 = await FileIO.ReadTextAsync(DataFile);
            }
            catch { }
        }

        public static async void UpdateScore()
        {
            int highScore = Convert.ToInt16(MainPage.STRHighScore1);

            if (MainPage.MyScore > highScore)
            {
                try
                {
                    StorageFile DataFile = await StorageFolder.GetFileAsync(filename);
                    await FileIO.WriteTextAsync(DataFile, MainPage.MyScore.ToString());
                    ReadFile();
                }
                catch { }
            }
        }
    }
}
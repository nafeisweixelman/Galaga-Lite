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
            int HighScore = Convert.ToInt16(MainPage.STRHighScore);
            if (MainPage.MyScore > HighScore)
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

/*
 * 
 * https://social.msdn.microsoft.com/Forums/en-US/1d80aecd-befe-408f-add8-7b7bfa27bbe9/check-if-file-is-empty-or-size-0-under-local-storage-in-windows-81-application-using-c?forum=winappswithcsharp
 * 
 * 
 */
using System;
using Windows.Storage;
using Windows.Storage.FileProperties;

namespace GalagaLite.Class
{
    class Storage
    {
        //High Score Saving location
        public static string filename = "GalagaLiteHighScore.txt";
        public static StorageFolder StorageFolder = ApplicationData.Current.LocalFolder;
        public static int highScore;

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
                BasicProperties basicProperties = await DataFile.GetBasicPropertiesAsync();
                var size = basicProperties.Size;

                if (size == 0)
                    await FileIO.WriteTextAsync(DataFile, MainPage.MyScore.ToString());

                MainPage.STRHighScore = await FileIO.ReadTextAsync(DataFile);

                highScore = Convert.ToInt16(MainPage.STRHighScore);
            }
            catch { }
        }

        public static async void UpdateScore()
        {
            if (MainPage.MyScore > highScore)
            {
                try
                {
                    StorageFile DataFile = await StorageFolder.GetFileAsync(filename);
                    await FileIO.WriteTextAsync(DataFile, MainPage.MyScore.ToString());
                    ReadFile();
                }
                catch
                { }
            }
        }
    }
}
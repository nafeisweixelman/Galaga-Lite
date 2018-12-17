/*
 * The game kept crashing when we tried to update the score for the first after the file was created. After some research we found that when the file was creatd
 * it only initialized with a null character which cannot be converted to an int and compared with the score. After further research we stumbled upon this site
 * https://social.msdn.microsoft.com/Forums/en-US/1d80aecd-befe-408f-add8-7b7bfa27bbe9/check-if-file-is-empty-or-size-0-under-local-storage-in-windows-81-application-using-c?forum=winappswithcsharp
 * which gave us information on how to check what the size of the file was to see if was empty or not.
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
        public static string filename = "GalagaLite.txt";
        public static string STRHighScore = "";
        public static StorageFolder StorageFolder = ApplicationData.Current.LocalFolder;
        public static int highScore;
        public static StorageFile DataFile;
        public static Boolean update = false;

        /// <summary>
        /// Create the Datafile or if it already exists to just open it
        /// </summary>
        public static async void CreateFile()
        {
            try
            {
                await StorageFolder.CreateFileAsync(filename, CreationCollisionOption.OpenIfExists);
            }
            catch { }
        }

        /// <summary>
        /// Reads the current folder and stores the information in DataFile. If the file is empty, meaning it has 0
        /// for its size, we write zero into the file to prevent the game from crashing
        /// </summary>
        public static async void ReadFile()
        {
            try
            {
                DataFile = await StorageFolder.GetFileAsync(filename);
                BasicProperties basicProperties = await DataFile.GetBasicPropertiesAsync();
                var size = basicProperties.Size;

                if (size == 0)
                    await FileIO.WriteTextAsync(DataFile, "0");

                STRHighScore = await FileIO.ReadTextAsync(DataFile);

                highScore = Convert.ToInt16(STRHighScore);
            }
            catch { }
        }

        /// <summary>
        /// Updates the file only if a new highscore is reached
        /// </summary>
        public static async void UpdateScore()
        {
            if (MainPage.MyScore > highScore)
            {
                try
                {
                    await FileIO.WriteTextAsync(DataFile, MainPage.MyScore.ToString());
                }
                catch { }

                update = true;
            }
        }
    }
}
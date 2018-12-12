﻿using System;
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

            if (MainPage.MyScore > Int32.Parse(MainPage.STRHighScore))
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

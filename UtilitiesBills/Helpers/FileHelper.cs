using System;
using System.ComponentModel;
using System.IO;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace UtilitiesBills.Helpers
{
    public class FileHelper
    {
        public static string GetLocalPath(string filename)
        {
            return Path.Combine(FileSystem.AppDataDirectory, filename);
        }

        public static string GetLocalCachePath(string filename)
        {
            return Path.Combine(FileSystem.CacheDirectory, filename);
        }

        /// <exception cref="System.NotImplementedException"></exception>
        /// <exception cref="System.ComponentModel.InvalidEnumArgumentException"></exception>
        public static string GetLocalApplicationDataPath()
        {
            switch (Device.RuntimePlatform)
            {
                case Device.Android:
                    return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                case Device.iOS:
                    throw new NotImplementedException("CreateZipFile isn't implemented for iOS.");
                default:
                    throw new InvalidEnumArgumentException(
                        $"CreateZipFile: Platform {Device.RuntimePlatform} is undefined.");
            }
        }
    }
}

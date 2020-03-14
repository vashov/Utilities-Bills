using System.IO;
using Xamarin.Essentials;

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
    }
}

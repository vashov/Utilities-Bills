using NLog;
using System;
using System.IO;
using System.IO.Compression;

namespace UtilitiesBills.Helpers
{
    public class ArchiverHelper
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public string CreateZipFileOfLogs()
        {
            if (!LogManager.IsLoggingEnabled())
            {
                return string.Empty;
            }

            string folder;
            try
            {
                folder = FileHelper.GetLocalApplicationDataPath();
            }
            catch (Exception ex)
            {
                _logger.Error($"Error getting local application data path: {ex.Message}");
                return string.Empty;
            }
            if (!DeleteOldZipFiles(folder))
            {
                return string.Empty;
            }
            
            string logFolder = Path.Combine(folder, "logs");

            if (!Directory.Exists(logFolder))
            {
                return string.Empty;
            }

            if (!AnyFilesForArchiving(logFolder))
            {
                return string.Empty;
            }

            string zipFilename = $"{folder}/{DateTime.Now.ToString("yyyyMMdd-HHmmss")}.zip";
            if (QuickZip(logFolder, zipFilename))
            {
                return zipFilename;
            }

            return string.Empty;
        }

        private bool AnyFilesForArchiving(string logFolder)
        {
            int filesCount = Directory.GetFiles(logFolder, "*.csv").Length;
            return filesCount > 0;
        }

        private bool DeleteOldZipFiles(string folder)
        {
            try
            {
                foreach (string fileName in Directory.GetFiles(folder, "*.zip"))
                {
                    File.Delete(fileName);
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Error deleting old zip files: {ex.Message}");
                return false;
            }
            return true;
        }

        private bool QuickZip(string directoryToZip, string destinationZipFullPath)
        {
            try
            {
                if (File.Exists(destinationZipFullPath))
                {
                    File.Delete(destinationZipFullPath);
                }
                if (!Directory.Exists(directoryToZip))
                {
                    return false;
                }
                else
                {
                    ZipFile.CreateFromDirectory(directoryToZip, destinationZipFullPath, CompressionLevel.Optimal, true);
                    return File.Exists(destinationZipFullPath);
                }
            }
            catch (Exception e)
            {
                _logger.Error($"Exception zipping files: {e.Message}");
                return false;
            }
        }
    }
}

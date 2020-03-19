using NLog;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using UtilitiesBills.Helpers;
using UtilitiesBills.ViewModels.Base;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace UtilitiesBills.ViewModels
{
    public class BackupInfoViewModel : BaseViewModel
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger(); 
        private string emailForSendBackup;

        public string EmailForSendBackup 
        { 
            get => emailForSendBackup;
            set
            {
                if (SetProperty(ref emailForSendBackup, value))
                {
                    SettingsService.EmailForSendBackup = value;
                    SendBackupToEmailCommand?.ChangeCanExecute();
                }
            }
        }

        public Command SendBackupToEmailCommand { get; private set; }
        public Command RestoreDatabaseCommand { get; private set; }

        public BackupInfoViewModel()
        {
            EmailForSendBackup = SettingsService.EmailForSendBackup;
            SendBackupToEmailCommand = new Command(SendBackupToEmail, CanSendBackupToEmail);
            RestoreDatabaseCommand = new Command(RestoreDatabase);
        }

        private bool CanSendBackupToEmail() => !string.IsNullOrWhiteSpace(EmailForSendBackup);

        private async void SendBackupToEmail()
        {
            string dbPath = SettingsService.DatabasePath;
            string backupFilePath = FileHelper.GetLocalCachePath("utilitiesBackup.db3");
            try
            {
                if (!File.Exists(backupFilePath))
                {
                    File.Create(backupFilePath).Close();
                }
                using (var conn = new SQLiteConnection(dbPath))
                {
                    conn.Backup(backupFilePath);
                }

                var message = new EmailMessage
                {
                    To = new List<string> { EmailForSendBackup },
                    Subject = "Backup",
                    Body = $"Send a backup ({DateTime.Now}).",
                };
                message.Attachments.Add(new EmailAttachment(backupFilePath));
                await Email.ComposeAsync(message);
            }
            catch(Exception ex)
            {
                LogHelper.LogErrorAndUserAlert(DialogService, _logger, ex, "Exception sending backup to email.");
            }
        }

        private async void RestoreDatabase()
        {
            string pathToBackupDatabase = null;
            try
            {
                using (FileData backupFileData = await CrossFilePicker.Current.PickFile())
                {
                    if (backupFileData == null)
                    {
                        return; // user canceled file picking
                    }

                    string backupFileName = backupFileData.FileName;

                    bool result = await DialogService.ShowQuestion(
                        $"Восстановить данные платежей из файла: {backupFileName}", "Подтверждение", "Да", "Нет");
                    if (!result)
                    {
                        return;
                    }

                    pathToBackupDatabase = FileHelper.GetLocalCachePath(backupFileName);
                    using (FileStream newBackupFile = File.Create(pathToBackupDatabase))
                    {
                        using (Stream backupFileStream = backupFileData.GetStream())
                        {
                            backupFileStream.CopyTo(newBackupFile);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogErrorAndUserAlert(DialogService, _logger, ex, "Exception choosing file.");
                return;
            }

            try
            {
                using (var conn = new SQLiteConnection(pathToBackupDatabase))
                {
                    conn.Backup(SettingsService.DatabasePath);
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogErrorAndUserAlert(DialogService, _logger, ex, "Exception backuping file.");
                return;
            }

            MessagingCenter.Send(this, MessageKeys.DatabaseRestored);
            await DialogService.ShowAlert("Данные успешно восстановленны", "Восстановление", "Ок");
        }
    }
}

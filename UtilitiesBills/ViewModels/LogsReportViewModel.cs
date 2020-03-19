using NLog;
using System;
using System.Collections.Generic;
using UtilitiesBills.Helpers;
using UtilitiesBills.ViewModels.Base;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace UtilitiesBills.ViewModels
{
    public class LogsReportViewModel : BaseViewModel
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private string _emailForSendLogs;

        public string EmailForSendLogs
        {
            get => _emailForSendLogs;
            set
            {
                if (SetProperty(ref _emailForSendLogs, value))
                {
                    UpdateEmailForSendLogsSettings();
                    ArchiveAndSendLogsCommand?.ChangeCanExecute();
                }
            }
        }

        public Command ArchiveAndSendLogsCommand { get; private set; }

        public LogsReportViewModel()
        {
            ArchiveAndSendLogsCommand = new Command(ArchiveAndSendLogs, CanArchiveAndSendLogs);
        }

        public override void Initialize(object navigationData)
        {
            EmailForSendLogs = SettingsService.EmailForSendLogs;
        }

        private void UpdateEmailForSendLogsSettings()
        {
            if (SettingsService.EmailForSendLogs != EmailForSendLogs)
            {
                SettingsService.EmailForSendLogs = EmailForSendLogs;
            }
        }

        private bool CanArchiveAndSendLogs() => !string.IsNullOrEmpty(EmailForSendLogs);

        private async void ArchiveAndSendLogs()
        {
            string zipFileName = CreateLogArchive();
            if (string.IsNullOrEmpty(zipFileName))
            {
                _logger.Warn("Archiv of logs wasn't created.");
                await DialogService.ShowAlert("Архив с логами не был создан.", "Ошибка", "Ок");
            }

            SendLogArchiveToEmail(zipFileName);
        }

        private string CreateLogArchive()
        {
            var archiver = new ArchiverHelper();
            string zipFileName = archiver.CreateZipFileOfLogs();
            return zipFileName;
        }

        private async void SendLogArchiveToEmail(string zipFileName)
        {
            try
            {
                var message = new EmailMessage
                {
                    To = new List<string> { EmailForSendLogs },
                    Subject = "Logs",
                    Body = $"Send logs ({DateTime.Now}).",
                };
                message.Attachments.Add(new EmailAttachment(zipFileName));
                await Email.ComposeAsync(message);
            }
            catch (Exception ex)
            {
                LogHelper.LogErrorAndUserAlert(DialogService, _logger, ex, "Exception sending logs to email.");
            }
        }
    }
}

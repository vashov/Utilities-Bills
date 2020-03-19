using NLog;
using System;
using UtilitiesBills.Services.Dialog;

namespace UtilitiesBills.Helpers
{
    public class LogHelper
    {
        public static async void LogErrorAndUserAlert(IDialogService dialog, ILogger logger, Exception ex, string message)
        {
            logger.Error(ex, message);
            await dialog.ShowAlert(message, "Error", "OK");
        }
    }
}

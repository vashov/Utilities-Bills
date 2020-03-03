using System;
using System.Text;
using System.Threading.Tasks;
using UtilitiesBills.Models;
using UtilitiesBills.Services.Dialog;
using Xamarin.Essentials;

namespace UtilitiesBills.Reports
{
    public class SmsSimpleReport
    {
        private readonly BillItem _bill;
        private readonly IDialogService _dialogService;
        public string ReportText { get; private set; }

        public SmsSimpleReport(IDialogService dialogService, BillItem bill)
        {
            if (bill == null || dialogService == null)
            {
                throw new ArgumentNullException();
            }

            _dialogService = dialogService;

            _bill = bill.Clone() as BillItem;
        }

        public void Generate()
        {
            var builder = new StringBuilder();

            builder.AppendLine("Значения счётчиков:");
            builder.AppendLine($"Хол. {_bill.ColdWaterCounterBulkRounded} ({_bill.ColdWaterExpenses} руб)");
            builder.AppendLine($"Гор. {_bill.HotWaterCounterBulkRounded} ({_bill.HotWaterExpenses} руб)");
            builder.AppendLine($"Эл. {_bill.ElectricityCounterBulkRounded} ({_bill.ElectricityExpenses} руб)");
            builder.AppendLine($"Водоотв. {_bill.WaterDisposalExpenses} руб");
            builder.AppendLine($"Итого: {_bill.TotalExpenses} руб");
            builder.AppendLine("По тем же тарифам.");
            builder.AppendLine("От предыдущих значений.");

            ReportText = builder.ToString();
        }

        public async Task CopyToBuffer()
        {
            await Clipboard.SetTextAsync(ReportText);
            await _dialogService.ShowAlert("Отчёт сформирован и скопирован в буфер", "Отчёт", "Ок");
        }
    }
}

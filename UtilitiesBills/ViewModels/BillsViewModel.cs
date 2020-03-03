using System.Collections.ObjectModel;
using System.Linq;
using UtilitiesBills.Models;
using UtilitiesBills.Reports;
using UtilitiesBills.Services;
using UtilitiesBills.ViewModels.Base;
using Xamarin.Forms;

namespace UtilitiesBills.ViewModels
{
    public class BillsViewModel : BaseViewModel
    {
        public ObservableCollection<BillItem> BillsItems { get; private set; } = 
            new ObservableCollection<BillItem>();

        public Command AddBillCommand { get; private set; }
        public Command<int> GenerateSmsReportCommand { get; private set; }

        public BillsViewModel()
        {
            SubscribeToModifyBillRepository();
            RefreshBillsItems();

            InitializeCommands();
        }

        public void OnBillTapped(object sender, ItemTappedEventArgs e)
        {
            var bill = e.Item as BillItem;
            var args = new BillEditorArgs
            {
                Bill = bill.Clone() as BillItem,
                PreviousBill = GetPreviousBill(bill)
            };
            NavigationService.NavigateTo<BillEditorViewModel>(args);
        }

        private void InitializeCommands()
        {
            AddBillCommand = new Command(AddBill);
            GenerateSmsReportCommand = new Command<int>(GenerateSmsReport);
        }

        private void AddBill()
        {
            var args = new BillEditorArgs
            {
                PreviousBill = GetLastBill()
            };
            NavigationService.NavigateTo<BillEditorViewModel>(args);
        }

        private async void GenerateSmsReport(int billId)
        {
            BillItem bill = BillsItems.First(b => b.Id == billId);
            var smsReport = new SmsSimpleReport(DialogService, bill);
            smsReport.Generate();
            await smsReport.CopyToBuffer();
        }

        private BillItem GetLastBill()
        {
            return BillsItems
                .OrderByDescending(b => b.DateOfReading)
                .First().Clone() as BillItem;
        }

        private BillItem GetPreviousBill(BillItem bill)
        {
            return BillsItems
                .Where(b => b.DateOfReading < bill.DateOfReading)
                .OrderByDescending(b => b.DateOfReading)
                .First().Clone() as BillItem;
        }

        private void SubscribeToModifyBillRepository()
        {
            MessagingCenter
                .Subscribe<IRepository<BillItem>>(this, MessageKeys.AddBillItem, (s) => RefreshBillsItems());
            MessagingCenter
                .Subscribe<IRepository<BillItem>>(this, MessageKeys.DeleteBillItem, (s) => RefreshBillsItems());
            MessagingCenter
                .Subscribe<IRepository<BillItem>>(this, MessageKeys.UpdateBillItem, (s) => RefreshBillsItems());
        }

        private void RefreshBillsItems()
        {
            BillsItems.Clear();
            IOrderedEnumerable<BillItem> bills = BillsRepository.GetItems()
                .OrderByDescending(b => b.DateOfReading);

            foreach (BillItem bill in bills)
            {
                BillsItems.Add(bill);
            }
        }
    }
}

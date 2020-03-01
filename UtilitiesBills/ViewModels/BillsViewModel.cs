using System.Collections.ObjectModel;
using System.Linq;
using UtilitiesBills.Models;
using UtilitiesBills.Services;
using UtilitiesBills.ViewModels.Base;
using Xamarin.Forms;

namespace UtilitiesBills.ViewModels
{
    public class BillsViewModel : BaseViewModel
    {
        public ObservableCollection<BillItem> BillsItems { get; set; } = new ObservableCollection<BillItem>();

        public Command AddBillCommand { get; set; }

        public BillsViewModel()
        {
            SubscribeToModifyBillRepository();
            RefreshBillsItems();

            InitializeCommands();
        }

        public async void OnBillTapped(object sender, ItemTappedEventArgs e)
        {
            var bill = e.Item as BillItem;
            var args = new BillEditorArgs
            {
                Bill = bill.Clone() as BillItem,
                PreviousMeterReading = GetPreviousMeterReading(bill)
            };
            await NavigationService.NavigateToAsync<BillEditorViewModel>(args);
        }

        private void InitializeCommands()
        {
            AddBillCommand = new Command(AddBill);
        }

        private async void AddBill()
        {
            var args = new BillEditorArgs
            {
                PreviousMeterReading = GetLastMeterReading()
            };
            await NavigationService.NavigateToAsync<BillEditorViewModel>(args);
        }

        private MeterReadingItem GetLastMeterReading()
        {
            return BillsItems
                .OrderByDescending(b => b.MeterReading.DateOfReading)
                .First().MeterReading.Clone() as MeterReadingItem;
        }

        private MeterReadingItem GetPreviousMeterReading(BillItem bill)
        {
            return BillsItems
                .Where(b => b.MeterReading.DateOfReading < bill.MeterReading.DateOfReading)
                .OrderByDescending(b => b.MeterReading.DateOfReading)
                .First().MeterReading.Clone() as MeterReadingItem;
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
                .OrderByDescending(b => b.MeterReading.DateOfReading);

            foreach (BillItem bill in bills)
            {
                BillsItems.Add(bill);
            }
        }
    }
}

using System.Collections.ObjectModel;
using System.Diagnostics;
using UtilitiesBills.Models;
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
            foreach (BillItem bill in BillsRepository.GetItems())
            {
                BillsItems.Add(bill);
            }

            InitializeCommands();
        }
        public void OnBillTapped(object sender, ItemTappedEventArgs e)
        {
            var bill = e.Item as BillItem;
            Debug.WriteLine("bill: " + bill.CreationDate.ToString("d")); // TODO add BillDetailPage or BillEditorPage
        }

        private void InitializeCommands()
        {
            AddBillCommand = new Command(AddBill);
        }

        private void AddBill()
        {
            Debug.WriteLine("You try add new bill"); // TODO add NewBillPage or BillEditorPage
        }
    }
}

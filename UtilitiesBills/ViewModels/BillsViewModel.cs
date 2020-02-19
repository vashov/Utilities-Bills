using System.Collections.ObjectModel;
using System.Diagnostics;
using UtilitiesBills.Models;
using Xamarin.Forms;

namespace UtilitiesBills.ViewModels
{
    public class BillsViewModel : BaseViewModel
    {
        public ObservableCollection<Bill> BillsItems { get; set; } = new ObservableCollection<Bill>();

        public Command AddBillCommand { get; set; }

        public BillsViewModel()
        {
            foreach (Bill bill in BillsRepository.GetItems())
            {
                BillsItems.Add(bill);
            }

            InitializeCommands();
        }
        public void OnBillTapped(object sender, ItemTappedEventArgs e)
        {
            var bill = e.Item as Bill;
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

using UtilitiesBills.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UtilitiesBills.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BillsPage : ContentPage
    {
        public BillsPage()
        {
            BindingContextChanged += BillsPage_BindingContextChanged;
            InitializeComponent();
        }

        private void BillsPage_BindingContextChanged(object sender, System.EventArgs e)
        {
            var vm = BindingContext as BillsViewModel;
            BillList.ItemTapped += vm.OnBillTapped;
        }
    }
}

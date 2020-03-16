using System.Linq;
using UtilitiesBills.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UtilitiesBills.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuView : ContentPage
    {
        public MenuView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Hack: Highlight selected (first by default) in Master page.
        /// </summary>
        public void SetFirstMenuItemAsSelected()
        {
            Models.MenuItem selectedItem = (BindingContext as MenuViewModel).MenuItems.FirstOrDefault();
            MenuList.SelectedItem = selectedItem;
        }
    }
}

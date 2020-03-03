using System.Threading.Tasks;
using Xamarin.Forms;

namespace UtilitiesBills.Services.Dialog
{
    public class DialogService : IDialogService
    {
        private Page CurrentPage => ((App.Current.MainPage as MasterDetailPage).Detail as NavigationPage).CurrentPage;

        public Task ShowAlert(string message, string title, string buttonLabel)
        {
            return CurrentPage.DisplayAlert(title, message, buttonLabel);
        }

        public async Task<bool> ShowQuestion(string message, string title, string accept, string cancel)
        {
            return await CurrentPage.DisplayAlert(title, message, accept, cancel);
        }
    }
}

using System.Threading.Tasks;

namespace UtilitiesBills.Services.Dialog
{
    public interface IDialogService
    {
        Task ShowAlert(string message, string title, string buttonLabel);

        Task<bool> ShowQuestion(string message, string title, string accept, string cancel);
    }
}

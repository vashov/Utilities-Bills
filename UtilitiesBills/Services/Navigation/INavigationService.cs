using System.Threading.Tasks;
using UtilitiesBills.Models;
using UtilitiesBills.ViewModels.Base;
using Xamarin.Forms;

namespace UtilitiesBills.Services.Navigation
{
    public interface INavigationService
    {
        /// <summary>
        /// Выполняет переход на одну из двух страниц при запуске приложения.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Выполняет иерархическую навигацию на указанной странице.
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        void NavigateTo<TViewModel>() where TViewModel : BaseViewModel;

        /// <summary>
        /// Выполняет иерархическую навигацию на указанной странице, передавая параметр.
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        /// <param name="parameter"></param>
        void NavigateTo<TViewModel>(object parameter) where TViewModel : BaseViewModel;

        /// <summary>
        /// Навигация с помощью меню MasterDetail.
        /// </summary>
        void NavigateFromMenu(MenuItemType id);

        /// <summary>
        /// Вернуться по стеку навигации на предыдущую страницу.
        /// </summary>
        void GoBack();
    }
}

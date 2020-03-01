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
        Task NavigateToAsync<TViewModel>() where TViewModel : BaseViewModel;

        /// <summary>
        /// Выполняет иерархическую навигацию на указанной странице, передавая параметр.
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        /// <param name="parameter"></param>
        Task NavigateToAsync<TViewModel>(object parameter) where TViewModel : BaseViewModel;

        /// <summary>
        /// Удаляет предыдущую страницу из стека навигации.
        /// </summary>
        void RemoveLastFromBackStack();

        /// <summary>
        /// Удаляет все предыдущие страницы из стека навигации.
        /// </summary>
        void RemoveBackStack();

        /// <summary>
        /// Навигация с помощью меню MasterDetail.
        /// </summary>
        void NavigateFromMenu(MenuItemType id);

        void GoBack();

        Task NavigateToModalAsync<TViewModel>() where TViewModel : BaseViewModel;
        Task NavigateToModalAsync<TViewModel>(object parameter) where TViewModel : BaseViewModel;
    }
}

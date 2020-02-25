using UtilitiesBills.ViewModels.Base;

namespace UtilitiesBills.Services.Navigation
{
    public interface INavigationService
    {
        BaseViewModel PreviousPageViewModel { get; set; }

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
        /// Удаляет предыдущую страницу из стека навигации.
        /// </summary>
        void RemoveLastFromBackStack();

        /// <summary>
        /// Удаляет все предыдущие страницы из стека навигации.
        /// </summary>
        void RemoveBackStack();
    }
}

using System;
using UtilitiesBills.ViewModels.Base;

namespace UtilitiesBills.Services.Navigation
{
    public class NavigationService : INavigationService
    {
        public BaseViewModel PreviousPageViewModel { get; set; }

        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public void NavigateTo<TViewModel>() where TViewModel : BaseViewModel
        {
            throw new NotImplementedException();
        }

        public void NavigateTo<TViewModel>(object parameter) where TViewModel : BaseViewModel
        {
            throw new NotImplementedException();
        }

        public void RemoveBackStack()
        {
            throw new NotImplementedException();
        }

        public void RemoveLastFromBackStack()
        {
            throw new NotImplementedException();
        }
    }
}

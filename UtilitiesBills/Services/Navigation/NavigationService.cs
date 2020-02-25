using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using UtilitiesBills.Models;
using UtilitiesBills.ViewModels;
using UtilitiesBills.ViewModels.Base;
using Xamarin.Forms;

namespace UtilitiesBills.Services.Navigation
{
    public class NavigationService : INavigationService
    {
        private MasterDetailPage MasterDetailPage => App.Current.MainPage as MasterDetailPage;

        private readonly Dictionary<MenuItemType, NavigationPage> MenuNavigationPages = 
            new Dictionary<MenuItemType, NavigationPage>();

        public BaseViewModel PreviousPageViewModel { get; set; }

        public void Initialize()
        {
            var vm = MasterDetailPage.Master.BindingContext as BaseViewModel;
            vm.Initialize(null);
        }

        public void NavigateFromMenu(MenuItemType id)
        {
            if (!MenuNavigationPages.ContainsKey(id))
            {
                switch (id)
                {
                    case MenuItemType.Bills:
                        Page billPage = CreatePage(typeof(BillsViewModel));
                        MenuNavigationPages.Add(id, new NavigationPage(billPage));
                        break;
                    case MenuItemType.Settings:
                        Page settingsPage = CreatePage(typeof(SettingsViewModel));
                        MenuNavigationPages.Add(id, new NavigationPage(settingsPage));
                        break;
                    case MenuItemType.Charts:
                        throw new NotImplementedException(nameof(MenuItemType.Charts));
                    default:
                        throw new InvalidEnumArgumentException(nameof(id), (int)id, typeof(MenuItemType));
                }
            }

            NavigationPage newNavPage = MenuNavigationPages[id];
            if (MasterDetailPage.Detail != newNavPage)
            {
                MasterDetailPage.Detail = newNavPage;
                //if (Device.RuntimePlatform == Device.Android)
                //    await Task.Delay(100);
                MasterDetailPage.IsPresented = false;
            }

            var vm = newNavPage.CurrentPage.BindingContext as BaseViewModel;
            vm.Initialize(null);
        }

        public void NavigateTo<TViewModel>() where TViewModel : BaseViewModel
        {
            InternalNavigateTo(typeof(TViewModel), null);
        }

        public void NavigateTo<TViewModel>(object parameter) where TViewModel : BaseViewModel
        {
            InternalNavigateTo(typeof(TViewModel), parameter);
        }

        /*public ViewModelBase PreviousPageViewModel
        {
            get
            {
                var mainPage = Application.Current.MainPage as CustomNavigationView;
                var viewModel = mainPage.Navigation.NavigationStack[mainPage.Navigation.NavigationStack.Count - 2].BindingContext;
                return viewModel as ViewModelBase;
            }
        }*/

        public void RemoveLastFromBackStack()
        {
            var detail = MasterDetailPage.Detail;

            if (detail != null)
            {
                detail.Navigation.RemovePage(
                    detail.Navigation.NavigationStack[detail.Navigation.NavigationStack.Count - 2]);
            }
        }

        public void RemoveBackStack()
        {
            var detail = MasterDetailPage.Detail;

            if (detail != null)
            {
                for (int i = 0; i < detail.Navigation.NavigationStack.Count - 1; i++)
                {
                    var page = detail.Navigation.NavigationStack[i];
                    detail.Navigation.RemovePage(page);
                }
            }
        }
        
        private async void InternalNavigateTo(Type viewModelType, object parameter)
        {
            Page page = CreatePage(viewModelType);

            //if (page is MainMasterDetailView)
            //{
            //    Application.Current.MainPage = page;
            //    return;
            //}

            var detailPage = MasterDetailPage.Detail;
            if (detailPage != null)
            {
                await detailPage.Navigation.PushAsync(page);
                //Application.Current.MainPage = new CustomNavigationView(page);
            }
            else
            {
                throw new InvalidNavigationException("Error InternalNavigateTo");
            }

            (page.BindingContext as BaseViewModel).Initialize(parameter);
        }

        private Page CreatePage(Type viewModelType)
        {
            Type pageType = GetPageTypeForViewModel(viewModelType);
            if (pageType == null)
            {
                throw new Exception($"Cannot locate page type for {viewModelType}");
            }
            Page page = Activator.CreateInstance(pageType) as Page;
            return page;
        }

        private Type GetPageTypeForViewModel(Type viewModelType)
        {
            var viewName = viewModelType.FullName.Replace("Model", string.Empty);
            var viewModelAssemblyName = viewModelType.GetTypeInfo().Assembly.FullName;
            var viewAssemblyName = string.Format(CultureInfo.InvariantCulture, "{0}, {1}", viewName, viewModelAssemblyName);
            var viewType = Type.GetType(viewAssemblyName);
            return viewType;
        }
    }
}

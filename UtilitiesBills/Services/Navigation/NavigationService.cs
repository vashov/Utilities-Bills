using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using UtilitiesBills.Models;
using UtilitiesBills.ViewModels.Base;
using UtilitiesBills.Views;
using Xamarin.Forms;

namespace UtilitiesBills.Services.Navigation
{
    public class NavigationService : INavigationService
    {
        private static MasterDetailPage _defaultMasterDetailPage = null;

        private MasterDetailPage MasterDetailPage => App.Current.MainPage as MasterDetailPage;

        private readonly Dictionary<MenuItemType, NavigationPage> MenuNavigationPages = 
            new Dictionary<MenuItemType, NavigationPage>();

        public void Initialize()
        {
            if (_defaultMasterDetailPage == null)
            {
                var detail = AddNavigationPageIfNotExists(MenuItemType.Bills) as NavigationPage;
                _defaultMasterDetailPage = new MasterDetailPage
                {
                    Master = new MenuView(),
                    Detail = detail
                };
            }

            App.Current.MainPage = _defaultMasterDetailPage;
        }

        public Page AddNavigationPageIfNotExists(MenuItemType id)
        {
            if (!MenuNavigationPages.ContainsKey(id))
            {
                switch (id)
                {
                    case MenuItemType.Bills:
                        MenuNavigationPages.Add(id, new NavigationPage(new BillsView()));
                        break;
                    case MenuItemType.Settings:
                        MenuNavigationPages.Add(id, new NavigationPage(new SettingsView()));
                        break;
                    case MenuItemType.Charts:
                        throw new NotImplementedException(nameof(MenuItemType.Charts));
                    default:
                        throw new InvalidEnumArgumentException(nameof(id), (int)id, typeof(MenuItemType));
                }
            }

            return MenuNavigationPages[id];
        }

        public void NavigateFromMenu(MenuItemType id)
        {
            Page newNavPage = AddNavigationPageIfNotExists(id);
            try
            {
                if (MasterDetailPage.Detail != newNavPage)
                {
                    MasterDetailPage.Detail = newNavPage;
                    MasterDetailPage.IsPresented = false;

                    var vm = (newNavPage as NavigationPage).CurrentPage.BindingContext as BaseViewModel;
                    vm.Initialize(null);
                }
            }
            catch (Exception e)
            {
                string ms = e.Message;
                Debug.WriteLine(ms);
            }
        }

        public void NavigateTo<TViewModel>() where TViewModel : BaseViewModel
        {
            InternalNavigateTo(typeof(TViewModel), null);
        }

        public void NavigateTo<TViewModel>(object parameter) where TViewModel : BaseViewModel
        {
            InternalNavigateTo(typeof(TViewModel), parameter);
        }

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

            var detailPage = MasterDetailPage.Detail;
            if (detailPage != null)
            {
                await detailPage.Navigation.PushAsync(page);
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

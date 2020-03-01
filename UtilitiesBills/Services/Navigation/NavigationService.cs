using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;
using UtilitiesBills.Models;
using UtilitiesBills.ViewModels.Base;
using UtilitiesBills.Views;
using Xamarin.Forms;

namespace UtilitiesBills.Services.Navigation
{
    public class NavigationService : INavigationService
    {
        private MasterDetailPage _defaultMasterDetailPage = null;
        private readonly Dictionary<MenuItemType, NavigationPage> _menuNavigationPages = 
            new Dictionary<MenuItemType, NavigationPage>();

        private MasterDetailPage MasterDetailPage => App.Current.MainPage as MasterDetailPage;

        public void Initialize()
        {
            if (_defaultMasterDetailPage == null)
            {
                var detail = AddNavigationPageIfNotExists(MenuItemType.Bills);
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
            if (!_menuNavigationPages.ContainsKey(id))
            {
                switch (id)
                {
                    case MenuItemType.Bills:
                        _menuNavigationPages.Add(id, new NavigationPage(new BillsView()));
                        break;
                    case MenuItemType.Settings:
                        _menuNavigationPages.Add(id, new NavigationPage(new SettingsView()));
                        break;
                    case MenuItemType.Charts:
                        throw new NotImplementedException(nameof(MenuItemType.Charts));
                    default:
                        throw new InvalidEnumArgumentException(nameof(id), (int)id, typeof(MenuItemType));
                }
            }

            return _menuNavigationPages[id];
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
                Debug.WriteLine(e.Message);
            }
        }

        public async Task NavigateToAsync<TViewModel>() where TViewModel : BaseViewModel
        {
            await InternalNavigateToAsync(typeof(TViewModel), null);
        }

        public async Task NavigateToAsync<TViewModel>(object parameter) where TViewModel : BaseViewModel
        {
            await InternalNavigateToAsync(typeof(TViewModel), parameter);
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

        public void GoBack()
        {
            var detail = MasterDetailPage.Detail;

            if (detail != null)
            {
                detail.Navigation.PopAsync();
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
        
        private async Task InternalNavigateToAsync(Type viewModelType, object parameter, bool modal = false)
        {
            Page page = CreatePage(viewModelType);

            var detailPage = MasterDetailPage.Detail;
            if (detailPage != null)
            {
                if (modal)
                {
                    await detailPage.Navigation.PushModalAsync(page);
                }
                else
                {
                    await detailPage.Navigation.PushAsync(page);
                }
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

        public async Task NavigateToModalAsync<TViewModel>() where TViewModel : BaseViewModel
        {
            await InternalNavigateToAsync(typeof(TViewModel), null, modal: true);
        }

        public async Task NavigateToModalAsync<TViewModel>(object parameter) where TViewModel : BaseViewModel
        {
            await InternalNavigateToAsync(typeof(TViewModel), parameter, modal: true);
        }
    }
}

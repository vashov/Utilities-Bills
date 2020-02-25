using System.Collections.Generic;
using System.Linq;
using UtilitiesBills.Models;
using UtilitiesBills.ViewModels.Base;

namespace UtilitiesBills.ViewModels
{
    public class MenuViewModel : BaseViewModel
    {
        private MenuItem _selectedMenuItem;

        public List<MenuItem> MenuItems { get; set; }

        public MenuItem SelectedMenuItem
        {
            get => _selectedMenuItem;
            set
            {
                SetProperty(ref _selectedMenuItem, value, OnSelectMenuItem);
            }
        }

        public MenuViewModel()
        {
            MenuItems = new List<MenuItem>
            {
                new MenuItem
                {
                    MenuType = MenuItemType.Bills, 
                    Title = "Bills",
                    IsAvailable = true
                },
                new MenuItem
                {
                    MenuType = MenuItemType.Charts,
                    Title = "Charts"
                },
                new MenuItem
                { 
                    MenuType = MenuItemType.Settings,
                    Title = "Settings",
                    IsAvailable = true
                }
            };
        }

        public override void Initialize(object navigationData)
        {
            base.Initialize(navigationData);

            SelectedMenuItem = MenuItems.First(x => x.IsAvailable);
        }

        private void OnSelectMenuItem()
        {
            NavigationService.NavigateFromMenu(SelectedMenuItem.MenuType);
        }
    }
}

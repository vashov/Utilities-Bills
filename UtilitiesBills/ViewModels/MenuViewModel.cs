﻿using System.Collections.ObjectModel;
using UtilitiesBills.Models;
using UtilitiesBills.ViewModels.Base;

namespace UtilitiesBills.ViewModels
{
    public class MenuViewModel : BaseViewModel
    {
        private MenuItem _selectedMenuItem;

        public ObservableCollection<MenuItem> MenuItems { get; set; }

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
            MenuItems = new ObservableCollection<MenuItem>
            {
                new MenuItem
                {
                    MenuType = MenuItemType.Bills, 
                    Title = "Список платежей",
                    IconSource = "bills"
                },
                new MenuItem
                { 
                    MenuType = MenuItemType.Settings,
                    Title = "Настройки",
                    IconSource = "settings"
                }
            };
        }

        private void OnSelectMenuItem()
        {
            NavigationService.NavigateFromMenu(SelectedMenuItem.MenuType);
        }
    }
}

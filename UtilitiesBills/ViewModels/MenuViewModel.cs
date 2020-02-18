using System;
using System.Collections.Generic;
using System.Text;
using UtilitiesBills.Models;

namespace UtilitiesBills.ViewModels
{
    public class MenuViewModel : BaseViewModel
    {
        public List<MenuItem> MenuItems { get; set; }

        public MenuViewModel()
        {
            MenuItems = new List<MenuItem>
            {
                new MenuItem
                {
                    MenuType = MenuItemType.Bills, 
                    Title = "Bills"
                },
                new MenuItem
                {
                    MenuType = MenuItemType.Charts,
                    Title = "Charts"
                },
                new MenuItem
                { 
                    MenuType = MenuItemType.Options,
                    Title = "Options"
                }
            };
        }
    }
}

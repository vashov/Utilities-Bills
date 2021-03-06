﻿using Xamarin.Forms;

namespace UtilitiesBills.Models
{
    public enum MenuItemType
    {
        Bills,
        Charts,
        Settings
    }

    public class MenuItem
    {
        public MenuItemType MenuType { get; set; }
        public string Title { get; set; }
        public string IconSource { get; set; }
    }
}

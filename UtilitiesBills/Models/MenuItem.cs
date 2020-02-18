using System;
using System.Collections.Generic;
using System.Text;

namespace UtilitiesBills.Models
{
    public enum MenuItemType
    {
        Bills,
        Charts,
        Options
    }

    public class MenuItem
    {
        public MenuItemType MenuType { get; set; }
        public string Title { get; set; }
    }
}

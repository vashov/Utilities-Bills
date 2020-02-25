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
        public bool IsAvailable { get; set; }
    }
}

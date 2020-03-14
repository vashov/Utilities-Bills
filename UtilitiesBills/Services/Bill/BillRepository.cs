using SQLite;
using System.Collections.Generic;
using UtilitiesBills.Models;
using UtilitiesBills.Services.Settings;
using UtilitiesBills.ViewModels.Base;
using Xamarin.Forms;

namespace UtilitiesBills.Services.Bill
{
    public class BillRepository : IRepository<BillItem>
    {
        private readonly string _dbPath;

        public BillRepository(ISettingsService settingsService)
        {
            _dbPath = settingsService.DatabasePath;
            using (var connection = new SQLiteConnection(_dbPath))
            {
                connection.CreateTable<BillItem>();
            }
        }

        public bool AddItem(BillItem item)
        {
            using (var connection = new SQLiteConnection(_dbPath))
            {
                int insertedCount = connection.Insert(item, typeof(BillItem));
                if (insertedCount == 1)
                {
                    MessagingCenter.Send<IRepository<BillItem>>(this, MessageKeys.AddBillItem);
                    return true;
                }
                return false;
            }
        }

        public bool DeleteItem(int id)
        {
            using (var connection = new SQLiteConnection(_dbPath))
            {
                BillItem billForDeleting = connection.Table<BillItem>().FirstOrDefault(bill => bill.Id == id);
                if (billForDeleting == null)
                {
                    return false;
                }

                connection.Delete<BillItem>(id);
                MessagingCenter.Send<IRepository<BillItem>>(this, MessageKeys.DeleteBillItem);
                return true;
            }
        }

        public BillItem GetItem(int id)
        {
            using (var connection = new SQLiteConnection(_dbPath))
            {
                return connection.Table<BillItem>().FirstOrDefault(bill => bill.Id == id);
            }
        }

        public IEnumerable<BillItem> GetItems()
        {
            using (var connection = new SQLiteConnection(_dbPath))
            {
                return connection.Table<BillItem>().ToList();
            }
        }

        public bool UpdateItem(BillItem item)
        {
            using (var connection = new SQLiteConnection(_dbPath))
            {
                BillItem billForUpdating = connection.Table<BillItem>().FirstOrDefault(bill => bill.Id == item.Id);
                if (billForUpdating == null)
                {
                    return false;
                }

                connection.Update(item, typeof(BillItem));
                MessagingCenter.Send<IRepository<BillItem>>(this, MessageKeys.UpdateBillItem);
                return true;
            }
        }
    }
}

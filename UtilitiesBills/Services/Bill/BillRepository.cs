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
            using (SQLiteConnection _connection = new SQLiteConnection(_dbPath))
            {
                _connection.CreateTable<BillItem>();
            }
        }

        public bool AddItem(BillItem item)
        {
            using (SQLiteConnection connection = new SQLiteConnection(_dbPath))
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
            using (var _connection = new SQLiteConnection(_dbPath))
            {
                BillItem billForDeleting = _connection.Table<BillItem>().FirstOrDefault(bill => bill.Id == id);
                if (billForDeleting == null)
                {
                    return false;
                }

                _connection.Delete<BillItem>(id);
                MessagingCenter.Send<IRepository<BillItem>>(this, MessageKeys.DeleteBillItem);
                return true;
            }
        }

        public BillItem GetItem(int id)
        {
            using (var _connection = new SQLiteConnection(_dbPath))
            {
                return _connection.Table<BillItem>().FirstOrDefault(bill => bill.Id == id);
            }
        }

        public IEnumerable<BillItem> GetItems()
        {
            using (var _connection = new SQLiteConnection(_dbPath))
            {
                return _connection.Table<BillItem>().ToList();
            }
        }

        public bool UpdateItem(BillItem item)
        {
            using (var _connection = new SQLiteConnection(_dbPath))
            {
                BillItem billForUpdating = _connection.Table<BillItem>().FirstOrDefault(bill => bill.Id == item.Id);
                if (billForUpdating == null)
                {
                    return false;
                }

                _connection.Update(item, typeof(BillItem));
                MessagingCenter.Send<IRepository<BillItem>>(this, MessageKeys.UpdateBillItem);
                return true;
            }
        }
    }
}

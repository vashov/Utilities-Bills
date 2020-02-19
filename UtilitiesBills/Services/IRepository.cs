using System.Collections.Generic;

namespace UtilitiesBills.Services
{
    public interface IRepository<T>
    {
        bool AddItem(T item);
        bool UpdateItem(T item);
        bool DeleteItem(int id);
        T GetItem(int id);
        IEnumerable<T> GetItems();
    }
}

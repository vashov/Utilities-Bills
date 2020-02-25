using System;
using System.Collections.Generic;
using System.Linq;
using UtilitiesBills.Models;

namespace UtilitiesBills.Services.Bill
{
    public class MockBillRepository : IRepository<BillItem>
    {
        private readonly List<BillItem> _bills;

        public MockBillRepository()
        {
            _bills = new List<BillItem>
            {
                new BillItem
                {
                    Id = 1,
                    Note = "First bill.",
                    CreationDate = new DateTime(2019, 8, 15),
                    HotWaterValue = 304,
                    ColdWaterValue = 259,
                    ElectricityValue = 29609
                },
                new BillItem
                {
                    Id = 2,
                    CreationDate = new DateTime(2019, 9, 15),
                    HotWaterValue = 308,
                    ColdWaterValue = 272,
                    ElectricityValue = 29737
                },
                new BillItem
                {
                    Id = 3,
                    Note = "Some bill.",
                    CreationDate = new DateTime(2019, 10, 16),
                    HotWaterValue = 283,
                    ColdWaterValue = 314,
                    ElectricityValue = 29866
                },
                new BillItem
                {
                    Id = 4,
                    Note = "Okay bill",
                    CreationDate = new DateTime(2019, 11, 17),
                    HotWaterValue = 321,
                    ColdWaterValue = 294,
                    ElectricityValue = 29991
                },
                new BillItem
                {
                    Id = 5,
                    CreationDate = new DateTime(2019, 12, 15),
                    HotWaterValue = 329,
                    ColdWaterValue = 304,
                    ElectricityValue = 30114
                },
                new BillItem
                {
                    Id = 6,
                    CreationDate = new DateTime(2020, 1, 14),
                    HotWaterValue = 336,
                    ColdWaterValue = 313,
                    ElectricityValue = 30238
                },
                new BillItem
                {
                    Id = 7,
                    Note = "Bad idea",
                    CreationDate = new DateTime(2020, 2, 15),
                    HotWaterValue = 344,
                    ColdWaterValue = 324,
                    ElectricityValue = 30434
                }
            };

            foreach (BillItem bill in _bills)
            {
                bill.ColdWaterPrice = 18;
                bill.HotWaterPrice = 105;
                bill.ElectricityPrice = 2.6M;
                bill.WaterDisposalPrice = 13.8M;

                bill.WaterDisposalValue = bill.ColdWaterValue + bill.HotWaterValue;
                bill.TotalExpenses = GetTotalExpenses(bill);
            }
        }

        private decimal GetTotalExpenses(BillItem bill)
        {
            return bill.HotWaterValue * bill.HotWaterPrice
                + bill.ColdWaterValue * bill.ColdWaterPrice
                + bill.ElectricityValue * bill.ElectricityPrice
                + bill.WaterDisposalValue * bill.WaterDisposalPrice;
        }

        public bool AddItem(BillItem item)
        {
            _bills.Add(item);
            return true;
        }

        public bool DeleteItem(int id)
        {
            BillItem billForRemoving = _bills.FirstOrDefault(bill => bill.Id == id);
            if (billForRemoving == null)
            {
                return false;
            }

            _bills.Remove(billForRemoving);
            return true;
        }

        public BillItem GetItem(int id)
        {
            return _bills.FirstOrDefault(bill => bill.Id == id);
        }

        public IEnumerable<BillItem> GetItems()
        {
            return _bills;
        }

        public bool UpdateItem(BillItem item)
        {
            BillItem billForUpdating = _bills.FirstOrDefault(bill => bill.Id == item.Id);
            if (billForUpdating == null)
            {
                return false;
            }

            // TODO Update properties without removing
            _bills.Remove(billForUpdating);
            _bills.Add(item);
            return true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using UtilitiesBills.Models;
using UtilitiesBills.ViewModels.Base;
using Xamarin.Forms;

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
                    HotWaterValueRounded = 304,
                    ColdWaterValueRounded = 259,
                    ElectricityValueRounded = 29609,
                    DateOfReading = new DateTime(2019, 8, 15)
                },
                new BillItem
                {
                    Id = 2,
                    CreationDate = new DateTime(2019, 9, 15),
                    HotWaterValueRounded = 308,
                    ColdWaterValueRounded = 272,
                    ElectricityValueRounded = 29737,
                    DateOfReading = new DateTime(2019, 9, 15)
                },
                new BillItem
                {
                    Id = 3,
                    Note = "Some bill.",
                    CreationDate = new DateTime(2019, 10, 16),
                    HotWaterValueRounded = 283,
                    ColdWaterValueRounded = 314,
                    ElectricityValueRounded = 29866,
                    DateOfReading = new DateTime(2019, 10, 16)
                },
                new BillItem
                {
                    Id = 4,
                    Note = "Okay bill",
                    CreationDate = new DateTime(2019, 11, 17),
                    HotWaterValueRounded = 321,
                    ColdWaterValueRounded = 294,
                    ElectricityValueRounded = 29991,
                    DateOfReading = new DateTime(2019, 11, 17)
                },
                new BillItem
                {
                    Id = 5,
                    CreationDate = new DateTime(2019, 12, 15),
                    HotWaterValueRounded = 329,
                    ColdWaterValueRounded = 304,
                    ElectricityValueRounded = 30114,
                    DateOfReading = new DateTime(2019, 12, 15)
                },
                new BillItem
                {
                    Id = 6,
                    CreationDate = new DateTime(2020, 1, 14),
                    HotWaterValueRounded = 336,
                    ColdWaterValueRounded = 313,
                    ElectricityValueRounded = 30238,
                    DateOfReading = new DateTime(2020, 1, 14)
                },
                new BillItem
                {
                    Id = 7,
                    Note = "Bad idea",
                    CreationDate = new DateTime(2020, 2, 15),
                    HotWaterValueRounded = 344,
                    ColdWaterValueRounded = 324,
                    ElectricityValueRounded = 30434,
                    DateOfReading = new DateTime(2020, 2, 15)
                }
            };

            foreach (BillItem bill in _bills)
            {
                bill.ColdWaterPrice = 18;
                bill.HotWaterPrice = 105;
                bill.ElectricityPrice = 2.6M;
                bill.WaterDisposalPrice = 13.8M;

                bill.TotalExpenses = GetTotalExpenses(bill);
            }
        }

        private decimal GetTotalExpenses(BillItem bill)
        {
            return bill.HotWaterValueRounded * bill.HotWaterPrice
                + bill.ColdWaterValueRounded * bill.ColdWaterPrice
                + bill.ElectricityValueRounded * bill.ElectricityPrice
                + bill.WaterDisposalBulk * bill.WaterDisposalPrice;
        }

        public bool AddItem(BillItem item)
        {
            _bills.Add(item);
            MessagingCenter.Send<IRepository<BillItem>>(this, MessageKeys.AddBillItem);
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
            MessagingCenter.Send<IRepository<BillItem>>(this, MessageKeys.DeleteBillItem);
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
            MessagingCenter.Send<IRepository<BillItem>>(this, MessageKeys.UpdateBillItem);
            return true;
        }
    }
}

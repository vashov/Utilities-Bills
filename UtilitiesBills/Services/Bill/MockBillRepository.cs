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
        private readonly List<MeterReadingItem> _meterReadingsItems;

        public MockBillRepository()
        {
            _meterReadingsItems = new List<MeterReadingItem>
            {
                new MeterReadingItem
                {
                    Id = 1,
                    HotWaterBulk = 304,
                    ColdWaterBulk = 259,
                    ElectricityBulk = 29609,
                    DateOfReading = new DateTime(2019, 8, 15)
                },
                new MeterReadingItem
                {
                    Id = 2,
                    HotWaterBulk = 308,
                    ColdWaterBulk = 272,
                    ElectricityBulk = 29737,
                    DateOfReading = new DateTime(2019, 9, 15)
                },
                new MeterReadingItem
                {
                    Id = 3,
                    HotWaterBulk = 283,
                    ColdWaterBulk = 314,
                    ElectricityBulk = 29866,
                    DateOfReading = new DateTime(2019, 10, 16)
                },
                new MeterReadingItem
                {
                    Id = 4,
                    HotWaterBulk = 321,
                    ColdWaterBulk = 294,
                    ElectricityBulk = 29991,
                    DateOfReading = new DateTime(2019, 11, 17)
                },
                new MeterReadingItem
                {
                    Id = 5,
                    HotWaterBulk = 329,
                    ColdWaterBulk = 304,
                    ElectricityBulk = 30114,
                    DateOfReading = new DateTime(2019, 12, 15)
                },
                new MeterReadingItem
                {
                    Id = 6,
                    HotWaterBulk = 336,
                    ColdWaterBulk = 313,
                    ElectricityBulk = 30238,
                    DateOfReading = new DateTime(2020, 1, 14)
                },
                new MeterReadingItem
                {
                    Id = 7,
                    HotWaterBulk = 344,
                    ColdWaterBulk = 324,
                    ElectricityBulk = 30434,
                    DateOfReading = new DateTime(2020, 2, 15)
                }
            };

            _bills = new List<BillItem>
            {
                new BillItem
                {
                    Id = 1,
                    Note = "First bill.",
                    CreationDate = new DateTime(2019, 8, 15),
                    MeterReading = _meterReadingsItems.First(m => m.Id == 1)
                },
                new BillItem
                {
                    Id = 2,
                    CreationDate = new DateTime(2019, 9, 15),
                    MeterReading = _meterReadingsItems.First(m => m.Id == 2)
                },
                new BillItem
                {
                    Id = 3,
                    Note = "Some bill.",
                    CreationDate = new DateTime(2019, 10, 16),
                    MeterReading = _meterReadingsItems.First(m => m.Id == 3)
                },
                new BillItem
                {
                    Id = 4,
                    Note = "Okay bill",
                    CreationDate = new DateTime(2019, 11, 17),
                    MeterReading = _meterReadingsItems.First(m => m.Id == 4)
                },
                new BillItem
                {
                    Id = 5,
                    CreationDate = new DateTime(2019, 12, 15),
                    MeterReading = _meterReadingsItems.First(m => m.Id == 5)
                },
                new BillItem
                {
                    Id = 6,
                    CreationDate = new DateTime(2020, 1, 14),
                    MeterReading = _meterReadingsItems.First(m => m.Id == 6)
                },
                new BillItem
                {
                    Id = 7,
                    Note = "Bad idea",
                    CreationDate = new DateTime(2020, 2, 15),
                    MeterReading = _meterReadingsItems.First(m => m.Id == 7)
                }
            };

            foreach (BillItem bill in _bills)
            {
                bill.ColdWaterPrice = 18;
                bill.HotWaterPrice = 105;
                bill.ElectricityPrice = 2.6M;
                bill.WaterDisposalPrice = 13.8M;

                bill.WaterDisposalBulk = bill.MeterReading.ColdWaterBulk + bill.MeterReading.HotWaterBulk;
                bill.TotalExpenses = GetTotalExpenses(bill);
            }
        }

        private decimal GetTotalExpenses(BillItem bill)
        {
            return bill.MeterReading.HotWaterBulk * bill.HotWaterPrice
                + bill.MeterReading.ColdWaterBulk * bill.ColdWaterPrice
                + bill.MeterReading.ElectricityBulk * bill.ElectricityPrice
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

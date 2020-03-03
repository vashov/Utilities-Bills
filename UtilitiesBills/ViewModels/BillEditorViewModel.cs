﻿using System;
using UtilitiesBills.Models;
using UtilitiesBills.Validations;
using UtilitiesBills.Validations.Rules;
using UtilitiesBills.Validations.ViewModelValidators;
using UtilitiesBills.ViewModels.Base;
using Xamarin.Forms;

namespace UtilitiesBills.ViewModels
{
    public class BillEditorViewModel : EditorViewModel
    {
        private enum BulkType
        {
            HotWaterBulk,
            ColdWaterBulk,
            ElectricityBulk,
            WaterDisposalBulk
        }

        private BillItem _bill;
        private BillItem _prevBill;

        private ValidatableObject<decimal> _hotWaterCounterBulk;
        private ValidatableObject<decimal> _coldWaterCounterBulk;
        private ValidatableObject<decimal> _electricityCounterBulk;
        private ValidatableObject<string> _note;
        private bool _needSetPricesManually;
        private decimal _hotWaterPrice;
        private decimal _coldWaterPrice;
        private decimal _electricityPrice;
        private decimal _waterDisposalPrice;
        private DateTime _dateOfReading;
        private DateTime _creationDate;
        private DateTime? _editDate;
        private bool _isValid;
        private bool _isManuallyPrices;

        private decimal _hotWaterCounterBulkRounded;
        private decimal _coldWaterCounterBulkRounded;
        private decimal _electricityCounterBulkRounded;

        private decimal _hotWaterBulk;
        private decimal _coldWaterBulk;
        private decimal _electricityBulk;
        private decimal _waterDisposalBulk;

        private decimal _hotWaterSum;
        private decimal _coldWaterSum;
        private decimal _electricitySum;
        private decimal _waterDisposalSum;
        private decimal _totalSum;

        private bool IsStartEditEnabled => EditorMode == EditorMode.View;

        public bool IsSaveEnabled => EditorMode != EditorMode.View;

        public BillItem PrevBill
        {
            get => _prevBill;
            set => SetProperty(ref _prevBill, value);
        }

        public ValidatableObject<decimal> HotWaterCounterBulk
        {
            get => _hotWaterCounterBulk;
            set => SetProperty(ref _hotWaterCounterBulk, value);
        }

        public ValidatableObject<decimal> ColdWaterCounterBulk
        {
            get => _coldWaterCounterBulk;
            set => SetProperty(ref _coldWaterCounterBulk, value);
        }

        public ValidatableObject<decimal> ElectricityCounterBulk
        {
            get => _electricityCounterBulk;
            set => SetProperty(ref _electricityCounterBulk, value);
        }

        public ValidatableObject<string> Note
        {
            get => _note;
            set => SetProperty(ref _note, value);
        }

        public bool NeedSetPriceManually
        {
            get => _needSetPricesManually;
            set => SetProperty(ref _needSetPricesManually, value);
        }

        public decimal HotWaterPrice
        {
            get => _hotWaterPrice;
            set => SetProperty(ref _hotWaterPrice, value, CalcHotWaterSum);
        }

        public decimal ColdWaterPrice
        {
            get => _coldWaterPrice;
            set => SetProperty(ref _coldWaterPrice, value, CalcColdWaterSum);
        }

        public decimal ElectricityPrice
        {
            get => _electricityPrice;
            set => SetProperty(ref _electricityPrice, value, CalcElectricitySum);
        }

        public decimal WaterDisposalPrice
        {
            get => _waterDisposalPrice;
            set => SetProperty(ref _waterDisposalPrice, value, CalcWaterDisposalSum);
        }

        public DateTime DateOfReading
        {
            get => _dateOfReading;
            set => SetProperty(ref _dateOfReading, value);
        }

        public DateTime CreationDate
        {
            get => _creationDate;
            private set => SetProperty(ref _creationDate, value);
        }

        public DateTime? EditDate
        {
            get => _editDate;
            private set => SetProperty(ref _editDate, value);
        }

        public decimal HotWaterCounterBulkRounded
        {
            get => _hotWaterCounterBulkRounded;
            private set =>
                SetProperty(ref _hotWaterCounterBulkRounded, value, () => UpdateBulk(BulkType.HotWaterBulk));
        }
        
        public decimal ColdWaterCounterBulkRounded
        {
            get => _coldWaterCounterBulkRounded;
            private set => 
                SetProperty(ref _coldWaterCounterBulkRounded, value, () => UpdateBulk(BulkType.ColdWaterBulk));
        }

        public decimal ElectricityCounterBulkRounded
        {
            get => _electricityCounterBulkRounded;
            private set => 
                SetProperty(ref _electricityCounterBulkRounded, value, () => UpdateBulk(BulkType.ElectricityBulk));
        }

        public decimal HotWaterBulk 
        { 
            get => _hotWaterBulk;
            private set => SetProperty(ref _hotWaterBulk, value, 
                () => OnWaterBulkChanged(BulkType.HotWaterBulk));
        }

        public decimal ColdWaterBulk 
        { 
            get => _coldWaterBulk;
            private set => SetProperty(ref _coldWaterBulk, value, 
                () => OnWaterBulkChanged(BulkType.ColdWaterBulk));
        }

        public decimal ElectricityBulk 
        { 
            get => _electricityBulk;
            private set => SetProperty(ref _electricityBulk, value, CalcElectricitySum);
        }

        public decimal WaterDisposalBulk 
        { 
            get => _waterDisposalBulk;
            private set => SetProperty(ref _waterDisposalBulk, value, CalcWaterDisposalSum);
        }

        public decimal HotWaterSum 
        { 
            get => _hotWaterSum; 
            private set => SetProperty(ref _hotWaterSum, value); 
        }

        public decimal ColdWaterSum 
        { 
            get => _coldWaterSum; 
            private set => SetProperty(ref _coldWaterSum, value); 
        }

        public decimal ElectricitySum 
        { 
            get => _electricitySum; 
            private set => SetProperty(ref _electricitySum, value); 
        }

        public decimal WaterDisposalSum 
        { 
            get => _waterDisposalSum; 
            private set => SetProperty(ref _waterDisposalSum, value); 
        }

        public decimal TotalSum 
        { 
            get => _totalSum; 
            set => SetProperty(ref _totalSum, value); 
        }

        public bool IsValid 
        { 
            get => _isValid; 
            set => SetProperty(ref _isValid, value); 
        }

        public bool IsManuallyPrices 
        { 
            get => _isManuallyPrices; 
            set => SetProperty(ref _isManuallyPrices, value); 
        }

        public Command SaveBillCommand { get; private set; }
        public Command StartEditBillCommand { get; private set; }
        public Command DeleteBillCommand { get; private set; }

        public BillEditorViewModel()
        {
            _hotWaterCounterBulk = new ValidatableObject<decimal>(() 
                => HotWaterCounterBulkRounded = RoundBulk(HotWaterCounterBulk.Value));
            _coldWaterCounterBulk = new ValidatableObject<decimal>(() 
                => ColdWaterCounterBulkRounded = RoundBulk(ColdWaterCounterBulk.Value));
            _electricityCounterBulk = new ValidatableObject<decimal>(() 
                => ElectricityCounterBulkRounded = RoundBulk(ElectricityCounterBulk.Value));
            _note = new ValidatableObject<string>();

            var context = new BillEditorValidatorContext
            {
                HotWaterBulk = _hotWaterCounterBulk,
                ColdWaterBulk = _coldWaterCounterBulk,
                ElectricityBulk = _electricityCounterBulk,
                Note = _note
            };

            InitCommands();

            OnEditorModeChanged += EditorModeChanged;

            Validator = new BillEditorValidator(context);
            Validator.AddValidations();
        }

        public override void Initialize(object navigationData)
        {
            if (!(navigationData is BillEditorArgs args))
            {
                throw new ArgumentException();
            }

            PrevBill = args.PreviousBill;

            if (args.Bill != null)
            {
                _bill = args.Bill.Clone() as BillItem;

                HotWaterCounterBulk.Value = _bill.HotWaterCounterBulkRounded;
                ColdWaterCounterBulk.Value = _bill.ColdWaterCounterBulkRounded;
                ElectricityCounterBulk.Value = _bill.ElectricityCounterBulkRounded;
                Note.Value = _bill.Note;

                HotWaterPrice = _bill.HotWaterPrice;
                ColdWaterPrice = _bill.ColdWaterPrice;
                ElectricityPrice = _bill.ElectricityPrice;
                WaterDisposalPrice = _bill.WaterDisposalPrice;
                DateOfReading = _bill.DateOfReading.ToLocalTime();
                CreationDate = _bill.CreationDate.ToLocalTime();
                EditDate = _bill.EditDate?.ToLocalTime();

                return;
            }

            if (_bill == null)
            {
                EditorMode = EditorMode.Create;
                _bill = new BillItem();
                DateOfReading = DateTime.Now;
                CreationDate = DateTime.Now;

                HotWaterPrice = PriceService.HotWaterPrice;
                ColdWaterPrice = PriceService.ColdWaterPrice;
                ElectricityPrice = PriceService.ElectricityPrice;
                WaterDisposalPrice = PriceService.WaterDisposalPrice;
            }
        }

        private void InitCommands()
        {
            SaveBillCommand = new Command(SaveBill, () => IsSaveEnabled);
            StartEditBillCommand = new Command(StartEditBill, () => IsStartEditEnabled);
            DeleteBillCommand = new Command(DeleteBill, () => EditorMode == EditorMode.View);
        }

        private void SaveBill()
        {
            if (_bill == null)
            {
                throw new ArgumentNullException();
            }

            IsValid = true;
            bool isValidBill = Validator.Validate();

            if (!isValidBill)
            {
                IsValid = false;
                return;
            }

            _bill.HotWaterCounterBulkRounded = HotWaterCounterBulk.Value;
            _bill.ColdWaterCounterBulkRounded = ColdWaterCounterBulk.Value;
            _bill.ElectricityCounterBulkRounded = ElectricityCounterBulk.Value;
            _bill.Note = Note.Value;

            _bill.HotWaterBulk = HotWaterBulk;
            _bill.ColdWaterBulk = ColdWaterBulk;
            _bill.WaterDisposalBulk = WaterDisposalBulk;
            _bill.ElectricityBulk = ElectricityBulk;

            _bill.HotWaterExpenses = HotWaterSum;
            _bill.ColdWaterExpenses = ColdWaterSum;
            _bill.WaterDisposalExpenses = WaterDisposalSum;
            _bill.ElectricityExpenses = ElectricitySum;
            _bill.TotalExpenses = TotalSum;

            _bill.DateOfReading = DateOfReading;
            _bill.HotWaterPrice = HotWaterPrice;
            _bill.ColdWaterPrice = ColdWaterPrice;
            _bill.ElectricityPrice = ElectricityPrice;
            _bill.WaterDisposalPrice = WaterDisposalPrice;

            switch (EditorMode)
            {
                case EditorMode.Edit:
                    UpdateBill(_bill);
                    break;
                case EditorMode.Create:
                    CreateBill(_bill);
                    break;
                default:
                    throw new ArgumentException();
            }
            NavigationService.GoBack();
        }

        private async void DeleteBill()
        {
            if (_bill == null)
            {
                throw new ArgumentNullException();
            }
            if (await DialogService.ShowQuestion("Удалить счёт?", string.Empty, "Да", "Нет"))
            {
                BillsRepository.DeleteItem(_bill.Id);
                NavigationService.GoBack();
            }
        }

        private void CreateBill(BillItem bill)
        {
            if (bill == null)
            {
                throw new ArgumentNullException();
            }
            bill.CreationDate = CreationDate.ToUniversalTime();
            BillsRepository.AddItem(bill);
        }

        private void UpdateBill(BillItem bill)
        {
            if (bill == null)
            {
                throw new ArgumentNullException();
            }
            bill.EditDate = DateTime.UtcNow;
            BillsRepository.UpdateItem(bill);
        }

        private void StartEditBill()
        {
            EditorMode = EditorMode.Edit;
        }

        private void EditorModeChanged()
        {
            OnPropertyChanged(nameof(IsStartEditEnabled));
            OnPropertyChanged(nameof(IsSaveEnabled));
            SaveBillCommand.ChangeCanExecute();
            StartEditBillCommand.ChangeCanExecute();
            DeleteBillCommand.ChangeCanExecute();
        }

        private decimal RoundBulk(decimal bulk) => BillCalculatorService.RoundCounterBulk(bulk);

        private void CalcBulkExpense(BulkType netBulkType, decimal netBulk, decimal price)
        {
            decimal expense = BillCalculatorService.CalcBulkExpense(netBulk, price);
            switch (netBulkType)
            {
                case BulkType.HotWaterBulk:
                    HotWaterSum = expense;
                    return;
                case BulkType.ColdWaterBulk:
                    ColdWaterSum = expense;
                    return;
                case BulkType.ElectricityBulk:
                    ElectricitySum = expense;
                    return;
                case BulkType.WaterDisposalBulk:
                    WaterDisposalSum = expense;
                    return;
                default:
                    throw new ArgumentException();
            }
        }

        private void UpdateBulk(BulkType bulkType)
        {
            switch (bulkType)
            {
                case BulkType.HotWaterBulk:
                    HotWaterBulk = CalcBulk(_hotWaterCounterBulkRounded, PrevBill.HotWaterCounterBulkRounded);
                    return;
                case BulkType.ColdWaterBulk:
                    ColdWaterBulk = CalcBulk(_coldWaterCounterBulkRounded, PrevBill.ColdWaterCounterBulkRounded);
                    return;
                case BulkType.ElectricityBulk:
                    ElectricityBulk = CalcBulk(_electricityCounterBulkRounded, PrevBill.ElectricityCounterBulkRounded);
                    return;
                default:
                    throw new ArgumentException();
            }
        }

        private void OnWaterBulkChanged(BulkType bulkType)
        {
            UpdateWaterDisposalBulk();
            switch (bulkType)
            {
                case BulkType.HotWaterBulk:
                    CalcHotWaterSum();
                    return;
                case BulkType.ColdWaterBulk:
                    CalcColdWaterSum();
                    return;
                default:
                    throw new ArgumentException();
            }
        }

        private decimal CalcBulk(decimal roundedBulk, decimal prevBulk)
        {
            return BillCalculatorService.CalcBulk(roundedBulk, prevBulk);
        }

        private void CalcHotWaterSum()
        {
            CalcBulkExpense(BulkType.HotWaterBulk, HotWaterBulk, HotWaterPrice);
            UpdateTotalSum();
        }

        private void CalcColdWaterSum()
        {
            CalcBulkExpense(BulkType.ColdWaterBulk, ColdWaterBulk, ColdWaterPrice);
            UpdateTotalSum();
        }

        private void CalcWaterDisposalSum()
        {
            CalcBulkExpense(BulkType.WaterDisposalBulk, WaterDisposalBulk, WaterDisposalPrice);
            UpdateTotalSum();
        }

        private void CalcElectricitySum()
        {
            CalcBulkExpense(BulkType.ElectricityBulk, ElectricityBulk, ElectricityPrice);
            UpdateTotalSum();
        }

        private void UpdateWaterDisposalBulk()
        {
            WaterDisposalBulk = HotWaterBulk + ColdWaterBulk;
        }

        private void UpdateTotalSum()
        {
            decimal sum = HotWaterSum + ColdWaterSum + ElectricitySum + WaterDisposalSum;
            TotalSum = BillCalculatorService.RoundSum(sum);
        }
    }
}

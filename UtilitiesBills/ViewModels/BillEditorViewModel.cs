using System;
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

        private ValidatableObject<decimal> _hotWaterValue;
        private ValidatableObject<decimal> _coldWaterValue;
        private ValidatableObject<decimal> _electricityValue;
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

        private decimal _hotWaterValueRounded;
        private decimal _coldWaterValueRounded;
        private decimal _electricityValueRounded;

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

        public ValidatableObject<decimal> HotWaterValue
        {
            get => _hotWaterValue;
            set => SetProperty(ref _hotWaterValue, value);
        }

        public ValidatableObject<decimal> ColdWaterValue
        {
            get => _coldWaterValue;
            set => SetProperty(ref _coldWaterValue, value);
        }

        public ValidatableObject<decimal> ElectricityValue
        {
            get => _electricityValue;
            set => SetProperty(ref _electricityValue, value);
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

        public decimal HotWaterValueRounded
        {
            get => _hotWaterValueRounded;
            private set
            {
                SetProperty(ref _hotWaterValueRounded, value, () => 
                    HotWaterBulk = CalcBulk(_hotWaterValueRounded, PrevBill.HotWaterValueRounded));
            }
        }
        
        public decimal ColdWaterValueRounded
        {
            get => _coldWaterValueRounded;
            private set
            {
                SetProperty(ref _coldWaterValueRounded, value, () => 
                    ColdWaterBulk = CalcBulk(_coldWaterValueRounded, PrevBill.ColdWaterValueRounded));
            }
        }

        public decimal ElectricityValueRounded
        {
            get => _electricityValueRounded;
            private set
            {
                SetProperty(ref _electricityValueRounded, value, () => 
                    ElectricityBulk = CalcBulk(_electricityValueRounded, PrevBill.ElectricityValueRounded));
            }
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
            _hotWaterValue = 
                new ValidatableObject<decimal>(() => HotWaterValueRounded = RoundBulk(HotWaterValue.Value));
            _coldWaterValue = 
                new ValidatableObject<decimal>(() => ColdWaterValueRounded = RoundBulk(ColdWaterValue.Value));
            _electricityValue = 
                new ValidatableObject<decimal>(() => ElectricityValueRounded = RoundBulk(ElectricityValue.Value));
            _note = new ValidatableObject<string>();

            var context = new BillEditorValidatorContext
            {
                HotWaterBulk = _hotWaterValue,
                ColdWaterBulk = _coldWaterValue,
                ElectricityBulk = _electricityValue,
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

                HotWaterValue.Value = _bill.HotWaterValueRounded;
                ColdWaterValue.Value = _bill.ColdWaterValueRounded;
                ElectricityValue.Value = _bill.ElectricityValueRounded;
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

            _bill.HotWaterValueRounded = HotWaterValue.Value;
            _bill.ColdWaterValueRounded = ColdWaterValue.Value;
            _bill.ElectricityValueRounded = ElectricityValue.Value;
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

        private decimal RoundBulk(decimal bulk) => BillCalculatorService.RoundBulkValue(bulk);

        private decimal CalcBulk(decimal roundedBulk, decimal prevBulk)
        {
            return BillCalculatorService.CalcBulk(roundedBulk, prevBulk);
        }

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

        private void OnWaterBulkChanged(BulkType netBulkType)
        {
            UpdateWaterDisposalBulk();
            switch (netBulkType)
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

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
        private enum NetBulkType
        {
            HotWaterBulk,
            ColdWaterBulk,
            ElectricityBulk,
            WaterDisposalBulk
        }

        private BillItem _bill;
        private MeterReadingItem _prevMeterReading;

        private ValidatableObject<decimal> _hotWaterBulk;
        private ValidatableObject<decimal> _coldWaterBulk;
        private ValidatableObject<decimal> _electricityBulk;
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

        private decimal _hotWaterBulkRounded;
        private decimal _coldWaterBulkRounded;
        private decimal _electricityBulkRounded;

        private decimal _netHotWaterBulk;
        private decimal _netColdWaterBulk;
        private decimal _netElectricityBulk;
        private decimal _waterDisposalBulk;

        private decimal _hotWaterSum;
        private decimal _coldWaterSum;
        private decimal _electricitySum;
        private decimal _waterDisposalSum;
        private decimal _totalSum;

        private bool IsStartEditEnabled => EditorMode == EditorMode.View;

        public bool IsSaveEnabled => EditorMode != EditorMode.View;

        public MeterReadingItem PrevMeterReading
        {
            get => _prevMeterReading;
            set => SetProperty(ref _prevMeterReading, value);
        }

        public ValidatableObject<decimal> HotWaterBulk
        {
            get => _hotWaterBulk;
            set => SetProperty(ref _hotWaterBulk, value);
        }

        public ValidatableObject<decimal> ColdWaterBulk
        {
            get => _coldWaterBulk;
            set => SetProperty(ref _coldWaterBulk, value);
        }

        public ValidatableObject<decimal> ElectricityBulk
        {
            get => _electricityBulk;
            set => SetProperty(ref _electricityBulk, value);
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

        public decimal HotWaterBulkRounded
        {
            get => _hotWaterBulkRounded;
            private set
            {
                SetProperty(ref _hotWaterBulkRounded, value, () => 
                    NetHotWaterBulk = CalcNetBulk(_hotWaterBulkRounded, PrevMeterReading.HotWaterBulk));
            }
        }
        
        public decimal ColdWaterBulkRounded
        {
            get => _coldWaterBulkRounded;
            private set
            {
                SetProperty(ref _coldWaterBulkRounded, value, () => 
                    NetColdWaterBulk = CalcNetBulk(_coldWaterBulkRounded, PrevMeterReading.ColdWaterBulk));
            }
        }

        public decimal ElectricityBulkRounded
        {
            get => _electricityBulkRounded;
            private set
            {
                SetProperty(ref _electricityBulkRounded, value, () => 
                    NetElectricityBulk = CalcNetBulk(_electricityBulkRounded, PrevMeterReading.ElectricityBulk));
            }
        }

        public decimal NetHotWaterBulk 
        { 
            get => _netHotWaterBulk;
            private set => SetProperty(ref _netHotWaterBulk, value, 
                () => OnWaterBulkChanged(NetBulkType.HotWaterBulk));
        }

        public decimal NetColdWaterBulk 
        { 
            get => _netColdWaterBulk;
            private set => SetProperty(ref _netColdWaterBulk, value, 
                () => OnWaterBulkChanged(NetBulkType.ColdWaterBulk));
        }

        public decimal NetElectricityBulk 
        { 
            get => _netElectricityBulk;
            private set => SetProperty(ref _netElectricityBulk, value, CalcElectricitySum);
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
            _hotWaterBulk = 
                new ValidatableObject<decimal>(() => HotWaterBulkRounded = RoundBulk(HotWaterBulk.Value));
            _coldWaterBulk = 
                new ValidatableObject<decimal>(() => ColdWaterBulkRounded = RoundBulk(ColdWaterBulk.Value));
            _electricityBulk = 
                new ValidatableObject<decimal>(() => ElectricityBulkRounded = RoundBulk(ElectricityBulk.Value));
            _note = new ValidatableObject<string>();

            var context = new BillEditorValidatorContext
            {
                HotWaterBulk = _hotWaterBulk,
                ColdWaterBulk = _coldWaterBulk,
                ElectricityBulk = _electricityBulk,
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

            PrevMeterReading = args.PreviousMeterReading;

            if (args.Bill != null)
            {
                _bill = args.Bill.Clone() as BillItem;

                HotWaterBulk.Value = _bill.MeterReading.HotWaterBulk;
                ColdWaterBulk.Value = _bill.MeterReading.ColdWaterBulk;
                ElectricityBulk.Value = _bill.MeterReading.ElectricityBulk;
                Note.Value = _bill.Note;

                HotWaterPrice = _bill.HotWaterPrice;
                ColdWaterPrice = _bill.ColdWaterPrice;
                ElectricityPrice = _bill.ElectricityPrice;
                WaterDisposalPrice = _bill.WaterDisposalPrice;
                DateOfReading = _bill.MeterReading.DateOfReading.ToLocalTime();
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

            _bill.MeterReading.HotWaterBulk = HotWaterBulk.Value;
            _bill.MeterReading.ColdWaterBulk = ColdWaterBulk.Value;
            _bill.MeterReading.ElectricityBulk = ElectricityBulk.Value;
            _bill.Note = Note.Value;

            _bill.MeterReading.DateOfReading = DateOfReading;
            _bill.HotWaterPrice = HotWaterPrice;
            _bill.ColdWaterPrice = ColdWaterPrice;
            _bill.ElectricityPrice = ElectricityPrice;
            _bill.WaterDisposalPrice = WaterDisposalPrice;
            _bill.TotalExpenses = TotalSum;

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

        private decimal RoundBulk(decimal bulk) => BillCalculatorService.RoundBulk(bulk);

        private decimal CalcNetBulk(decimal roundedBulk, decimal prevBulk)
        {
            return BillCalculatorService.CalcNetBulk(roundedBulk, prevBulk);
        }

        private void CalcBulkExpense(NetBulkType netBulkType, decimal netBulk, decimal price)
        {
            decimal expense = BillCalculatorService.CalcBulkExpense(netBulk, price);
            switch (netBulkType)
            {
                case NetBulkType.HotWaterBulk:
                    HotWaterSum = expense;
                    return;
                case NetBulkType.ColdWaterBulk:
                    ColdWaterSum = expense;
                    return;
                case NetBulkType.ElectricityBulk:
                    ElectricitySum = expense;
                    return;
                case NetBulkType.WaterDisposalBulk:
                    WaterDisposalSum = expense;
                    return;
                default:
                    throw new ArgumentException();
            }
        }

        private void OnWaterBulkChanged(NetBulkType netBulkType)
        {
            UpdateWaterDisposalBulk();
            switch (netBulkType)
            {
                case NetBulkType.HotWaterBulk:
                    CalcHotWaterSum();
                    return;
                case NetBulkType.ColdWaterBulk:
                    CalcColdWaterSum();
                    return;
                default:
                    throw new ArgumentException();
            }
        }

        private void CalcHotWaterSum()
        {
            CalcBulkExpense(NetBulkType.HotWaterBulk, NetHotWaterBulk, HotWaterPrice);
            UpdateTotalSum();
        }

        private void CalcColdWaterSum()
        {
            CalcBulkExpense(NetBulkType.ColdWaterBulk, NetColdWaterBulk, ColdWaterPrice);
            UpdateTotalSum();
        }

        private void CalcWaterDisposalSum()
        {
            CalcBulkExpense(NetBulkType.WaterDisposalBulk, WaterDisposalBulk, WaterDisposalPrice);
            UpdateTotalSum();
        }

        private void CalcElectricitySum()
        {
            CalcBulkExpense(NetBulkType.ElectricityBulk, NetElectricityBulk, ElectricityPrice);
            UpdateTotalSum();
        }

        private void UpdateWaterDisposalBulk()
        {
            WaterDisposalBulk = NetHotWaterBulk + NetColdWaterBulk;
        }

        private void UpdateTotalSum()
        {
            decimal sum = HotWaterSum + ColdWaterSum + ElectricitySum + WaterDisposalSum;
            TotalSum = BillCalculatorService.RoundSum(sum);
        }
    }
}

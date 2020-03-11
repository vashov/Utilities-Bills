using UtilitiesBills.ViewModels.Base;
using Xamarin.Forms;

namespace UtilitiesBills.ViewModels
{
    public class DefaultPricesEditorViewModel : EditorViewModel
    {
        private decimal _hotWaterPrice;
        private decimal _coldWaterPrice;
        private decimal _electricityPrice;
        private decimal _waterDisposalPrice;

        public bool IsSaveEnabled => EditorMode != EditorMode.View;

        public decimal HotWaterPrice
        {
            get => _hotWaterPrice;
            set => SetProperty(ref _hotWaterPrice, value);
        }

        public decimal ColdWaterPrice
        {
            get => _coldWaterPrice;
            set => SetProperty(ref _coldWaterPrice, value);
        }

        public decimal ElectricityPrice
        {
            get => _electricityPrice;
            set => SetProperty(ref _electricityPrice, value);
        }

        public decimal WaterDisposalPrice 
        { 
            get => _waterDisposalPrice; 
            set => SetProperty(ref _waterDisposalPrice, value); 
        }

        public Command SaveCommand { get; private set; }
        public Command StartEditCommand { get; private set; }

        public DefaultPricesEditorViewModel()
        {
            InitCommands();
            OnEditorModeChanged += EditorModeChanged;
        }

        public override void Initialize(object navigationData)
        {
            HotWaterPrice = SettingsService.DefaultHotWaterPrice;
            ColdWaterPrice = SettingsService.DefaultColdWaterPrice;
            ElectricityPrice = SettingsService.DefaultElectricityPrice;
            WaterDisposalPrice = SettingsService.DefaultWaterDisposalPrice;
        }

        private void InitCommands()
        {
            SaveCommand = new Command(Save, () => IsSaveEnabled);
            StartEditCommand = new Command(StartEdit, () => EditorMode == EditorMode.View);
        }

        private void StartEdit() => EditorMode = EditorMode.Edit;

        private void Save()
        {
            // TODO: add validation
            SettingsService.DefaultHotWaterPrice = HotWaterPrice;
            SettingsService.DefaultColdWaterPrice = ColdWaterPrice;
            SettingsService.DefaultElectricityPrice = ElectricityPrice;
            SettingsService.DefaultWaterDisposalPrice = WaterDisposalPrice;

            NavigationService.GoBack();
        }

        private void EditorModeChanged()
        {
            OnPropertyChanged(nameof(IsSaveEnabled));
            SaveCommand.ChangeCanExecute();
            StartEditCommand.ChangeCanExecute();
        }
    }
}

using UtilitiesBills.ViewModels.Base;
using Xamarin.Forms;

namespace UtilitiesBills.ViewModels
{
    public class InitialCounterEditorViewModel : EditorViewModel
    {
        private decimal _hotWater;
        private decimal _coldWater;
        private decimal _electricity;

        public bool IsSaveEnabled => EditorMode != EditorMode.View;

        public decimal HotWater
        {
            get => _hotWater;
            set => SetProperty(ref _hotWater, value);
        }

        public decimal ColdWater 
        { 
            get => _coldWater; 
            set => SetProperty(ref _coldWater, value); 
        }

        public decimal Electricity 
        { 
            get => _electricity; 
            set => SetProperty(ref _electricity, value); 
        }

        public Command SaveCommand { get; private set; }
        public Command StartEditCommand { get; private set; }

        public InitialCounterEditorViewModel()
        {
            InitCommands();
            OnEditorModeChanged += EditorModeChanged;
        }

        public override void Initialize(object navigationData)
        {
            HotWater = SettingsService.InitHotWaterBulk;
            ColdWater = SettingsService.InitColdWaterBulk;
            Electricity = SettingsService.InitElectricityBulk;
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
            SettingsService.InitHotWaterBulk = HotWater;
            SettingsService.InitColdWaterBulk = ColdWater;
            SettingsService.InitElectricityBulk = Electricity;
            
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

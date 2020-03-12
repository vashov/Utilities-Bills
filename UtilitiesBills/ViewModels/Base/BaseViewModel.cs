using UtilitiesBills.Models;
using UtilitiesBills.Services;
using UtilitiesBills.Services.BillCalculator;
using UtilitiesBills.Services.Dialog;
using UtilitiesBills.Services.Navigation;
using UtilitiesBills.Services.Price;
using UtilitiesBills.Services.Settings;
using UtilitiesBills.Validations.ViewModelValidators;

namespace UtilitiesBills.ViewModels.Base
{
    public class BaseViewModel : BaseNotifier
    {
        protected IRepository<BillItem> BillsRepository { get; private set; }
        protected INavigationService NavigationService { get; private set; }
        protected IDialogService DialogService { get; private set; }
        protected IBillCalculatorService BillCalculatorService { get; private set; }
        protected IPriceService PriceService { get; private set; }
        protected ISettingsService SettingsService { get; private set; }
        protected IValidator Validator { get; set; }

        public BaseViewModel()
        {
            BillsRepository = ViewModelLocator.Resolve<IRepository<BillItem>>();
            NavigationService = ViewModelLocator.Resolve<INavigationService>();
            DialogService = ViewModelLocator.Resolve<IDialogService>();
            PriceService = ViewModelLocator.Resolve<IPriceService>();
            BillCalculatorService = ViewModelLocator.Resolve<IBillCalculatorService>();
            SettingsService = ViewModelLocator.Resolve<ISettingsService>();
        }

        public virtual void Initialize(object navigationData)
        {
        }        
    }
}

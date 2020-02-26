using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UtilitiesBills.Models;
using UtilitiesBills.Services;
using UtilitiesBills.Services.Navigation;

namespace UtilitiesBills.ViewModels.Base
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        protected IRepository<BillItem> BillsRepository { get; set; }
        protected INavigationService NavigationService { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public BaseViewModel()
        {
            BillsRepository = ViewModelLocator.Resolve<IRepository<BillItem>>();
            NavigationService = ViewModelLocator.Resolve<INavigationService>();
        }

        public virtual void Initialize(object navigationData)
        {
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T field, T value, 
            Action callAfterChange = null,
            [CallerMemberName] string  propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return false;
            }

            field = value;
            callAfterChange?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}

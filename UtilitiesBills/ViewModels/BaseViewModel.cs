using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UtilitiesBills.Models;
using UtilitiesBills.Services;
using Xamarin.Forms;

namespace UtilitiesBills.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected IRepository<Bill> BillsRepository = DependencyService.Get<IRepository<Bill>>();

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

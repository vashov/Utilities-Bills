using Autofac;
using System;
using System.Globalization;
using System.Reflection;
using UtilitiesBills.Models;
using UtilitiesBills.Services;
using UtilitiesBills.Services.Bill;
using UtilitiesBills.Services.Navigation;
using Xamarin.Forms;

namespace UtilitiesBills.ViewModels.Base
{
    public static class ViewModelLocator
    {
        private static readonly IContainer _container;

        public static readonly BindableProperty AutoWireViewModelProperty = BindableProperty.CreateAttached(
            propertyName: "AutoWireViewModel", 
            returnType: typeof(bool), 
            declaringType: typeof(ViewModelLocator), 
            defaultValue: default(bool), 
            propertyChanged: OnAutoWireViewModelChanged
        );

        public static bool GetAutoWireViewModel(BindableObject bindable)
        {
            return (bool)bindable.GetValue(ViewModelLocator.AutoWireViewModelProperty);
        }

        public static void SetAutoWireViewModel(BindableObject bindable, bool value)
        {
            bindable.SetValue(ViewModelLocator.AutoWireViewModelProperty, value);
        }

        public static bool UseMockService { get; set; }

        static ViewModelLocator()
        {
            var builder = new ContainerBuilder();
            RegisterTypes(builder, true);

            _container = builder.Build();
        }

        private static void RegisterTypes(ContainerBuilder builder, bool useMockServices)
        {
            RegisterViewModels(builder);

            builder.RegisterType<NavigationService>().As<INavigationService>().SingleInstance();

            // Change injected dependencies
            if (useMockServices)
            {
                builder.RegisterType<MockBillRepository>().As<IRepository<BillItem>>();
                UseMockService = true;
            }
            else
            {
                throw new NotImplementedException();
                //builder.RegisterType<MockBillRepository>().As<IRepository<BillItem>>();

                //UseMockService = false;
            }
        }

        private static void RegisterViewModels(ContainerBuilder builder)
        {
            builder.RegisterType<MenuViewModel>();
            builder.RegisterType<BillsViewModel>();
            builder.RegisterType<SettingsViewModel>();
        }

        //public static void RegisterSingleton<TInterface, T>() where TInterface : class where T : class, TInterface
        //{
        //    _container.Register<TInterface, T>().AsSingleton();
        //}

        public static T Resolve<T>() where T : class
        {
            return _container.Resolve<T>();
        }

        private static void OnAutoWireViewModelChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is Element view))
            {
                return;
            }

            var viewType = view.GetType();
            var viewName = viewType.FullName.Replace(".Views.", ".ViewModels.");
            var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
            var viewModelName = string.Format(CultureInfo.InvariantCulture, "{0}Model, {1}", viewName, viewAssemblyName);

            var viewModelType = Type.GetType(viewModelName);
            if (viewModelType == null)
            {
                return;
            }
            var viewModel = _container.Resolve(viewModelType);
            view.BindingContext = viewModel;
        }
    }
}

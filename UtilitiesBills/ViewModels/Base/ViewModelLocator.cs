﻿using Autofac;
using System;
using System.Globalization;
using System.Reflection;
using UtilitiesBills.Models;
using UtilitiesBills.Services;
using UtilitiesBills.Services.Bill;
using UtilitiesBills.Services.BillCalculator;
using UtilitiesBills.Services.Dialog;
using UtilitiesBills.Services.Navigation;
using UtilitiesBills.Services.Price;
using UtilitiesBills.Services.Settings;
using Xamarin.Forms;

namespace UtilitiesBills.ViewModels.Base
{
    public static class ViewModelLocator
    {
        private static readonly IContainer _container;

        public static bool UseMockService { get; set; }

        public static readonly BindableProperty AutoWireViewModelProperty = BindableProperty.CreateAttached(
            propertyName: "AutoWireViewModel", 
            returnType: typeof(bool), 
            declaringType: typeof(ViewModelLocator), 
            defaultValue: default(bool), 
            propertyChanged: OnAutoWireViewModelChanged
        );

        static ViewModelLocator()
        {
            var builder = new ContainerBuilder();
            RegisterTypes(builder, useMockServices: false);

            _container = builder.Build();
        }

        public static bool GetAutoWireViewModel(BindableObject bindable)
        {
            return (bool)bindable.GetValue(ViewModelLocator.AutoWireViewModelProperty);
        }

        public static void SetAutoWireViewModel(BindableObject bindable, bool value)
        {
            bindable.SetValue(ViewModelLocator.AutoWireViewModelProperty, value);
        }

        public static T Resolve<T>() where T : class
        {
            return _container.Resolve<T>();
        }

        private static void RegisterTypes(ContainerBuilder builder, bool useMockServices)
        {
            RegisterViewModels(builder);

            builder.RegisterType<NavigationService>().As<INavigationService>().SingleInstance();
            builder.RegisterType<DialogService>().As<IDialogService>().SingleInstance();
            builder.RegisterType<BillCalculatorService>().As<IBillCalculatorService>().SingleInstance();
            builder.RegisterType<SettingsService>().As<ISettingsService>().SingleInstance();

            // Change injected dependencies
            if (useMockServices)
            {
                builder.RegisterType<MockBillRepository>().As<IRepository<BillItem>>().SingleInstance();
                builder.RegisterType<MockPriceService>().As<IPriceService>().SingleInstance();
                UseMockService = true;
            }
            else
            {
                builder.RegisterType<BillRepository>().As<IRepository<BillItem>>().SingleInstance();
                builder.RegisterType<PriceService>().As<IPriceService>().SingleInstance();
                UseMockService = false;
            }
        }

        private static void RegisterViewModels(ContainerBuilder builder)
        {
            builder.RegisterType<MenuViewModel>().AsSelf();
            builder.RegisterType<BillsViewModel>().AsSelf();
            builder.RegisterType<SettingsViewModel>().AsSelf();
            builder.RegisterType<BillEditorViewModel>().AsSelf();
            builder.RegisterType<InitialCounterEditorViewModel>().AsSelf();
            builder.RegisterType<BackupInfoViewModel>().AsSelf();
            builder.RegisterType<DefaultPricesEditorViewModel>().AsSelf();
            builder.RegisterType<LogsReportViewModel>().AsSelf();
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

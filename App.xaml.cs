using Autofac;
using Microsoft.Extensions.DependencyInjection;
using SnailPass.Data.API;
using SnailPass.Model.Interfaces;
using SnailPass.Services;
using SnailPass.View;
using SnailPass.View.Dialogs;
using SnailPass.ViewModel;
using System.Net.Http.Headers;
using System;
using System.Windows;
using System.Configuration;
using AutoUpdaterDotNET;
using SnailPass.Services;
using System.Reflection;

namespace SnailPass
{
    public partial class App : Application
    {
        private Window _startupWindow;
        private IContainer _container;

        public Window StartupWindow
        {
            get { return _startupWindow; }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _container = ContainerConfig.Configure();
            RegisterDialogs();

            using (ILifetimeScope scope = _container.BeginLifetimeScope())
            {
                LoginViewModel loginViewModel = scope.Resolve<LoginViewModel>();
                _startupWindow = new StartupWindow()
                {
                    DataContext = scope.Resolve<StartupViewModel>()
                };
            }

            StartupWindow.Show();

            StartupWindow.IsVisibleChanged += (s, ev) =>
            {
                if (StartupWindow.IsVisible == false && MainWindow.IsLoaded)
                {
                    using (ILifetimeScope scope = _container.BeginLifetimeScope())
                    {
                        AccountsViewModel accountsViewModel = scope.Resolve<AccountsViewModel>();
                        MainWindow = new MainWindow()
                        {
                            DataContext = scope.Resolve<ApplicationViewModel>()
                        };
                    }
                    StartupWindow.Close();
                    MainWindow.Show();
                }
            };

            base.OnStartup(e);
        }

        private void RegisterDialogs()
        {
            DialogService.RegisterDialog<AddAccountViewModel, AddAccountDialog>();
            DialogService.RegisterDialog<AddCustomFieldViewModel, AddCustomFieldDialog>();
            DialogService.RegisterDialog<EditAccountViewModel, EditAccountDialog>();
            DialogService.RegisterDialog<EditCustomFieldViewModel, EditCustomFieldDialog>();
            DialogService.RegisterDialog<TokenExpiredViewModel, TokenExpiredDialog>();
            DialogService.RegisterDialog<AddNoteViewModel, AddNoteDialog>();
        }
    }
}
using Autofac;
using Microsoft.Extensions.DependencyInjection;
using SnailPass_Desktop.Data.API;
using SnailPass_Desktop.Model.Interfaces;
using SnailPass_Desktop.Services;
using SnailPass_Desktop.View;
using SnailPass_Desktop.View.Dialogs;
using SnailPass_Desktop.ViewModel;
using System.Net.Http.Headers;
using System;
using System.Windows;
using System.Configuration;

namespace SnailPass_Desktop
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
            DialogService.RegisterDialog<AddNewAccountViewModel, AddNewAccountDialog>();
            DialogService.RegisterDialog<AddCustomFieldViewModel, AddCustomFieldDialog>();
            DialogService.RegisterDialog<EditAccountViewModel, EditAccountDialog>();
            DialogService.RegisterDialog<EditCustomFieldViewModel, EditCustomFieldDialog>();
        }
    }
}
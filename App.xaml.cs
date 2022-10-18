using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SnailPass_Desktop.Data.API;
using SnailPass_Desktop.Model;
using SnailPass_Desktop.Repositories;
using SnailPass_Desktop.View;
using SnailPass_Desktop.ViewModel;
using SnailPass_Desktop.ViewModel.Stores;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

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

            //using (ILifetimeScope scope = _container.BeginLifetimeScope())
            //{
            //    LoginViewModel loginViewModel = scope.Resolve<LoginViewModel>();
            //    _startupWindow = new StartupWindow()
            //    {
            //        DataContext = scope.Resolve<StartupViewModel>()
            //    };
            //}

            //StartupWindow.Show();

            //StartupWindow.IsVisibleChanged += (s, ev) =>
            //{
            //    if (StartupWindow.IsVisible == false && MainWindow.IsLoaded)
            //    {
                    using (ILifetimeScope scope = _container.BeginLifetimeScope())
                    {
                        AccountsViewModel accountsViewModel = scope.Resolve<AccountsViewModel>();
                        MainWindow = new MainWindow()
                        {
                            DataContext = scope.Resolve<ApplicationViewModel>()
                        };
                    }
                    //StartupWindow.Close();
                    MainWindow.Show();
            //    }
            //};

            base.OnStartup(e);
        }
    }
}
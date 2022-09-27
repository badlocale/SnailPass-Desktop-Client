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
        public Window StartupWindow { get { return _startupWindow; } }

        protected override void OnStartup(StartupEventArgs e)
        {
            NavigationStore navigationStoreStartup = new NavigationStore();
            UserIdentityStore userIdentityStore = new UserIdentityStore();
            navigationStoreStartup.CurrentViewModel = new LoginViewModel(navigationStoreStartup, userIdentityStore);
            navigationStoreStartup.TextHeader = "Login";

            _startupWindow = new StartupWindow()
            {
                DataContext = new StartupViewModel(navigationStoreStartup)
            };

            StartupWindow.Show();

            StartupWindow.IsVisibleChanged += (s, ev) =>
            {
                if (StartupWindow.IsVisible == false && MainWindow.IsLoaded)
                {
                    NavigationStore navigationStoreMain = new NavigationStore();
                    navigationStoreMain.CurrentViewModel = new AccountsViewModel(userIdentityStore);

                    MainWindow = new MainWindow()
                    {
                        DataContext = new ApplicationViewModel(navigationStoreMain, userIdentityStore)
                    };

                    StartupWindow.Close();
                    MainWindow.Show();
                }
            };

            base.OnStartup(e);
        }
    }
}
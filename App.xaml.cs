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
        //private void Application_Startup(object sender, StartupEventArgs e)
        //{
        //    var loginWindow = new LoginWindow();
        //    var registrationWindow = new RegistrationWindow();
        //    loginWindow.Show();

        //    loginWindow.IsVisibleChanged += (s, ev) =>
        //    {
        //        if (loginWindow.IsVisible == false && loginWindow.IsLoaded)
        //        {
        //            var mainWindow = new MainWindow();
        //            loginWindow.Close();
        //            mainWindow.Show();
        //        }
        //    };
        //}

        protected override void OnStartup(StartupEventArgs e)
        {
            NavigationStore navigationStore = new NavigationStore();
            navigationStore.CurrentViewModel = new LoginViewModel(navigationStore);
            navigationStore.TextHeader = "Login";

            MainWindow = new StartupWindow()
            {
                DataContext = new StartupViewModel(navigationStore)
            };

            MainWindow.Show();

            base.OnStartup(e);
        }
    }
}
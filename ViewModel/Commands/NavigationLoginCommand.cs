using SnailPass_Desktop.ViewModel.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnailPass_Desktop.ViewModel.Commands
{
    internal class NavigationLoginCommand : CommandBase
    {
        private readonly NavigationStore _navigationStore;

        public NavigationLoginCommand(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }

        public override void Execute(object? obj)
        {
            _navigationStore.CurrentViewModel = new LoginViewModel(_navigationStore);
            _navigationStore.TextHeader = "Login";
        }
    }
}

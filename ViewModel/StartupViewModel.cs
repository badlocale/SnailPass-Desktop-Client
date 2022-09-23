using SnailPass_Desktop.ViewModel.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnailPass_Desktop.ViewModel
{
    public class StartupViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;

        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;
        public string HeaderName => "Login";

        public StartupViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }
    }
}

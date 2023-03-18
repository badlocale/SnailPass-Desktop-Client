using Serilog;
using SnailPass.ViewModel.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnailPass.ViewModel
{
    public class HomeViewModel : ViewModelBase
    {
        public HomeViewModel(IUserIdentityStore identityStore, INavigationStore navigationStore, 
            ILogger logger)
        {

        }
    }
}

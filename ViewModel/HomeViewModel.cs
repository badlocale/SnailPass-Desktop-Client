using Serilog;
using SnailPass_Desktop.ViewModel.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnailPass_Desktop.ViewModel
{
    public class HomeViewModel : ViewModelBase
    {
        public HomeViewModel(IUserIdentityStore identityStore, INavigationStore navigationStore, 
            ILogger logger)
        {

        }
    }
}

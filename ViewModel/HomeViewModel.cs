﻿using Microsoft.Extensions.Logging;
using SnailPass_Desktop.ViewModel.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnailPass_Desktop.ViewModel
{
    internal class HomeViewModel : ViewModelBase
    {
        public HomeViewModel(IUserIdentityStore identityStore, INavigationStore navigationStore, 
            ILogger logger)
        {

        }
    }
}

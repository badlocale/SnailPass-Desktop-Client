using Serilog;
using SnailPass_Desktop.ViewModel.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnailPass_Desktop.ViewModel.Commands
{
    internal class NavigateCommand<TViewModel> : CommandBase
        where TViewModel : ViewModelBase
    {
        private readonly INavigationStore _navigationStore;
        private readonly Func<TViewModel> _createViewModel;
        private readonly ILogger _logger;

        public NavigateCommand(INavigationStore navigationStore, Func<TViewModel> createViewModel, 
            ILogger logger)
        {
            _navigationStore = navigationStore;
            _createViewModel = createViewModel;
            _logger = logger;
        }

        public override void Execute(object? obj)
        {
            _navigationStore.CurrentViewModel = _createViewModel();
        }
    }
}

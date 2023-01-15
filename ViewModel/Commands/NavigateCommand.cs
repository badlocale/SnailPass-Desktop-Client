using SnailPass_Desktop.ViewModel.Stores;
using System;

namespace SnailPass_Desktop.ViewModel.Commands
{
    public class NavigateCommand<TViewModel> : CommandBase
        where TViewModel : ViewModelBase
    {
        private readonly INavigationStore _navigationStore;
        private readonly Func<TViewModel> _createViewModel;
        private string _header;

        public NavigateCommand(INavigationStore navigationStore, Func<TViewModel> createViewModel, string header)
        {
            _navigationStore = navigationStore;
            _createViewModel = createViewModel;
            _header = header;
        }

        public override void Execute(object? obj)
        {
            _navigationStore.CurrentViewModel = _createViewModel();
            _navigationStore.TextHeader = _header;
        }
    }
}
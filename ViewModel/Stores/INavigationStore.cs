using System;

namespace SnailPass_Desktop.ViewModel.Stores
{
    public interface INavigationStore
    {
        ViewModelBase CurrentViewModel { get; set; }
        string TextHeader { get; set; }

        event Action? CurrentViewModelChange;
        event Action? TextHeaderChange;
    }
}
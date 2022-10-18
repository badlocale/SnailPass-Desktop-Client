using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnailPass_Desktop.ViewModel.Stores
{
    internal class NavigationStore : INavigationStore
    {
        public event Action? CurrentViewModelChange;
        public event Action? TextHeaderChange;

        private ViewModelBase _currentViewModel;
        private string _textHeader;

        public ViewModelBase CurrentViewModel
        {
            get { return _currentViewModel; }
            set
            {
                _currentViewModel = value;
                OnCurrentViewModelChange();
            }
        }

        public string TextHeader
        {
            get { return _textHeader; }
            set
            {
                _textHeader = value;
                OnTextHeaderChange();
            }
        }

        private void OnCurrentViewModelChange()
        {
            CurrentViewModelChange?.Invoke();
        }

        private void OnTextHeaderChange()
        {
            TextHeaderChange?.Invoke();
        }
    }
}

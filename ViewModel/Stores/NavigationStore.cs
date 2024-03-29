﻿using System;

namespace SnailPass.ViewModel.Stores
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

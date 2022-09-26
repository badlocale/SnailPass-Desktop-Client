﻿using SnailPass_Desktop.ViewModel.Stores;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SnailPass_Desktop.ViewModel
{
    public class StartupViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private bool _isViewVisible = true;

        public bool IsViewVisible
        {
            get { return _isViewVisible; }
            set
            {
                _isViewVisible = value;
                OnPropertyChanged();
            }
        }

        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;
        public string HeaderName => _navigationStore.TextHeader;

        public StartupViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModelChange += OnCurrentViewModelChange;
            _navigationStore.TextHeaderChange += OnTextHeaderChange;
            _navigationStore.CurrentViewModel.PropertyChanged += OnPropertyChangedVisibilityHandler;
        }

        private void OnPropertyChangedVisibilityHandler(object? s, PropertyChangedEventArgs e) 
        {
            if (e.PropertyName == "IsViewVisible")
            {
                IsViewVisible = false;
            }
        }

        private void OnTextHeaderChange()
        {
            OnPropertyChanged(nameof(HeaderName));
        }

        private void OnCurrentViewModelChange()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}

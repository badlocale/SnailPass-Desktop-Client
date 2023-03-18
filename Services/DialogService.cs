using Serilog;
using SnailPass_Desktop.View.Dialogs;
using SnailPass_Desktop.ViewModel;
using SnailPass_Desktop.ViewModel.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace SnailPass_Desktop.Services
{
    public class DialogService : IDialogService
    {
        public event EventHandler DialogOpened;
        public event EventHandler DialogClosed;

        private static IEnumerable<Type> _dialogTypes;
        private static Dictionary<Type, Type> _mappings = new();
        private List<Type> _openedDialogsTypes = new();

        private IViewModelFactory _viewModelFactory;
        private ILogger _logger;

        public bool IsAllDialogsClosed => !_openedDialogsTypes.Any();

        static DialogService()
        {
            _dialogTypes = from t in Assembly.GetExecutingAssembly().GetTypes()
                           where t.GetTypeInfo().GetCustomAttribute<DialogContentAttribute>() != null
                           select t;
        }

        public DialogService(IViewModelFactory viewModelFactory, ILogger logger)
        {
            _viewModelFactory = viewModelFactory;
            _logger = logger;
        }

        public static void RegisterDialog<TViewModel, TView>()
            where TViewModel : ViewModelBase
            where TView : UserControl
        {
            _mappings.Add(typeof(TViewModel), typeof(TView));
        }

        public bool? ShowDialog(string name, Action<string>? callback = null)
        {
            Type dialogContentType = _dialogTypes.First(t => t.Name == $"{name}");
            if (_openedDialogsTypes.Any(d => d == dialogContentType))
            {
                _logger.Warning($"Dialog window with type {dialogContentType.Name} is already opend.");
                return null;
            }

            DialogWindow window = ShowDialogInternal(dialogContentType, callback);

            return window.DialogResult;
        }

        public TViewModel? ShowDialog<TViewModel>(Action<string>? callback = null)
            where TViewModel : ViewModelBase
        {
            Type dialogContentType = _mappings[typeof(TViewModel)];
            if (_openedDialogsTypes.Any(d => d == dialogContentType))
            {
                _logger.Warning($"Dialog window with type {dialogContentType.Name} is already opend.");
                return null;
            }

            DialogWindow window = ShowDialogInternal(dialogContentType, callback, typeof(TViewModel));

            bool? result = window.DialogResult;
            FrameworkElement content = window.Content as FrameworkElement;

            if (result == true)
            {
                return (TViewModel)content.DataContext;
            }
            else
            {
                return null;
            }
        }

        private DialogWindow ShowDialogInternal(Type contentType, Action<string>? callback, Type? viewModelType = null)
        {
            DialogWindow dialog = new DialogWindow();
            FrameworkElement content = Activator.CreateInstance(contentType) as FrameworkElement;

            if (viewModelType != null)
            {
                object viewModel = _viewModelFactory.Create(viewModelType);
                content.DataContext = viewModel;
            }

            dialog.Width = content.Width;
            dialog.Height = content.Height;
            dialog.Content = content;

            EventHandler closeEventHandler = null;
            closeEventHandler = (sender, args) =>
            {
                _openedDialogsTypes.Remove(contentType);
                if (callback != null)
                {
                    callback(dialog.DialogResult?.ToString());
                }
                OnDialogClosed();
                dialog.Closed -= closeEventHandler;
            };
            dialog.Closed += closeEventHandler;

            _openedDialogsTypes.Add(contentType);
            OnDialogOpend();
            dialog.Activate();
            dialog.ShowDialog();

            return dialog;
        }

        private void OnDialogOpend()
        {
            DialogOpened?.Invoke(this, new EventArgs());
        }

        private void OnDialogClosed()
        {
            DialogClosed?.Invoke(this, new EventArgs());
        }
    }
}

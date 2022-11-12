using SnailPass_Desktop.View.Dialogs;
using SnailPass_Desktop.ViewModel.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SnailPass_Desktop.ViewModel.Services
{
    internal class DialogService : IDialogService
    {
        private IViewModelFactory _viewModelFactory;
        private static IEnumerable<Type> _dialogTypes;
        private static Dictionary<Type, Type> _mappings = new();

        static DialogService()
        {
            _dialogTypes = from t in Assembly.GetExecutingAssembly().GetTypes()
                           where t.GetTypeInfo().GetCustomAttribute<DialogContentAttribute>() != null
                           select t;
        }

        public DialogService(IViewModelFactory viewModelFactory)
        {
            _viewModelFactory = viewModelFactory;
        }

        public static void RegisterDialog<TViewModel, TView>()
            where TViewModel : ViewModelBase
            where TView : UserControl
        {
            _mappings.Add(typeof(TViewModel), typeof(TView));
        }

        public ViewModelBase? ShowDialog(string name, Action<string>? callback = null)
        {
            Type dialogContentType = _dialogTypes.First(t => t.Name == $"{name}");
            DialogWindow window = ShowDialogInternal(dialogContentType, callback);

            bool? result = window.DialogResult;
            if (result == true)
            {
                return (ViewModelBase)window.DataContext;
            }
            else
            {
                return null;
            }
        }

        public TViewModel? ShowDialog<TViewModel>(Action<string>? callback = null)
            where TViewModel : ViewModelBase
        {
            Type dialogContentType = _mappings[typeof(TViewModel)];
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
            else
            {
                return dialog;
            }

            dialog.Width = content.Width;
            dialog.Height = content.Height;
            dialog.Content = content;

            EventHandler closeEventHandler = null;
            closeEventHandler = (sender, args) =>
            {
                if (callback != null)
                {
                    callback(dialog.DialogResult.ToString());
                }
                dialog.Closed -= closeEventHandler;
            };
            dialog.Closed += closeEventHandler;

            dialog.ShowDialog();

            return dialog;
        }
    }
}

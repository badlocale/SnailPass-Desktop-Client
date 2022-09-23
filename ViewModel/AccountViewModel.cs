using SnailPass_Desktop.ViewModel.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SnailPass_Desktop.ViewModel
{
    public class AccountViewModel : ViewModelBase
    {
        public ICommand NavigateRegisterCommand { get; }

        public AccountViewModel(NavigationStore navigationStore)
        {
            NavigateRegisterCommand = new ViewModelCommand(ExecuteLoginCommand, CanExecuteLoginCommand);
        }

        private bool CanExecuteLoginCommand(object obj)
        {
            throw new NotImplementedException();
        }

        private void ExecuteLoginCommand(object obh)
        {
            throw new NotImplementedException();
        }
    }
}

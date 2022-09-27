using SnailPass_Desktop.Model;
using SnailPass_Desktop.ViewModel.Commands;
using SnailPass_Desktop.ViewModel.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SnailPass_Desktop.ViewModel
{
    public class AccountsViewModel : ViewModelBase
    {
        ObservableCollection<AccountModel> _accounts;

        public ICommand AddNewCommand { get; set; }
        public ICommand RemoveCommand { get; set; }
        public ICommand UpdateCommand { get; set; }

        public AccountsViewModel(UserIdentityStore identityStore)
        {
            AddNewCommand = new AddNewCommand();//
        }
    }
}

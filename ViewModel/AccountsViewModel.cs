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
    internal class AccountsViewModel : ViewModelBase
    {
        private ObservableCollection<AccountModel> _accounts;

        public ObservableCollection<AccountModel> Accounts
        { 
            get { return _accounts; }
            set { _accounts = value; }
        }

        public ICommand AddNewCommand { get; set; }
        public ICommand RemoveCommand { get; set; }
        public ICommand UpdateCommand { get; set; }

        public AccountsViewModel(UserIdentityStore identityStore)
        {
            AddNewCommand = new AddNewCommand();//
        }
    }
}

using SnailPass_Desktop.Model;
using SnailPass_Desktop.Repositories;
using SnailPass_Desktop.ViewModel.Commands;
using SnailPass_Desktop.ViewModel.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace SnailPass_Desktop.ViewModel
{
    internal class AccountsViewModel : ViewModelBase
    {
        private readonly ObservableCollection<AccountModel> _accounts;

        private IAccountRepository _repository;

        public ICommand AddNewCommand { get; set; }
        public ICommand RemoveCommand { get; set; }
        public ICommand UpdateCommand { get; set; }

        public ICollectionView AccountsCollectiionView { get; }

        public ObservableCollection<AccountModel> Accounts
        {
            get { return _accounts; }
        }

        public AccountsViewModel(UserIdentityStore identityStore)
        {
            _repository = new AccountRepository();

            _accounts = new ObservableCollection<AccountModel>();
            AccountsCollectiionView = CollectionViewSource.GetDefaultView(_accounts);
            AccountsCollectiionView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(AccountModel.ServiceName)));

            IEnumerable<AccountModel> accounts = _repository.GetByUserID("50b1fe14-6345-46ef-9b3d-477ff20a93f8");
            foreach (AccountModel account in accounts)
            {
                _accounts.Add(account);
            }

            Console.WriteLine(Accounts.Count());
        }
    }
}

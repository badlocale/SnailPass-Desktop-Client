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
        private string _searchBarText = string.Empty;
        private string _customFieldName;
        private string _customFieldValue;
        private IAccountRepository _repository;
        private IUserIdentityStore _identity;

        public ObservableCollection<AccountModel> Accounts
        {
            get { return _accounts; }
        }

        public string SearchBarText
        {
            get { return _searchBarText; }
            set
            {
                _searchBarText = value;
                OnPropertyChanged();
                AccountsCollectiionView.Refresh();
            }
        }

        public string CustomFieldName
        {
            get { return _customFieldName; }
            set
            {
                _customFieldName = value;
                OnPropertyChanged();
            }
        }

        public string CustomFieldValue
        {
            get { return _customFieldValue; }
            set
            {
                _customFieldValue = value;
                OnPropertyChanged();
            }
        }

        public ICommand RemoveCommand { get; set; }
        public ICommand AddNewCommand { get; set; }
        public ICommand UpdateCommand { get; set; }

        public ICollectionView AccountsCollectiionView { get; }

        public AccountsViewModel(IUserIdentityStore identityStore, INavigationStore navigationStore)
        {
            navigationStore.CurrentViewModel = this;

            _identity = identityStore;
            _repository = new AccountRepository();
            _accounts = new ObservableCollection<AccountModel>();

            AccountsCollectiionView = CollectionViewSource.GetDefaultView(_accounts);
            AccountsCollectiionView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(AccountModel.ServiceName)));
            AccountsCollectiionView.Filter = FilerWithSearchBar;

            //_identity.CurrentUser.ID = "50b1fe14-6345-46ef-9b3d-477ff20a93f8"; //DELETE
            IEnumerable<AccountModel> accounts = _repository.GetByUserID("50b1fe14-6345-46ef-9b3d-477ff20a93f8");
            foreach (AccountModel account in accounts)
            {
                _accounts.Add(account);
            }

            Console.WriteLine(Accounts.Count());
        }

        private bool FilerWithSearchBar(object obj)
        {
            if (obj is AccountModel account)
            {
                return account.ServiceName.Contains(SearchBarText) ||
                    account.Login.Contains(SearchBarText);
            }
            return false;
        }
    }
}

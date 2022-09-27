using SnailPass_Desktop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnailPass_Desktop.ViewModel.Commands
{
    internal class AddNewCommand : CommandBase
    {
        private ViewModelBase _viewModel;
        private IAccountRepository _repository;

        public AddNewCommand(IAccountRepository repository)
        {
            _repository = repository;
        }

        public override void Execute(object? parameter)
        {
            //AccountModel account = new AccountModel()
            //{
            //    ID = _viewModel;
            //}

            ////TODO net instead local
            //_repository.Add();
        }
    }
}

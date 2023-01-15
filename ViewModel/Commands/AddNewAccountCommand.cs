using Serilog;
using SnailPass_Desktop.Model.Interfaces;
using SnailPass_Desktop.ViewModel.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnailPass_Desktop.ViewModel.Commands
{
    public class AddNewAccountCommand : CommandBase
    {
        private AccountsViewModel _viewModel;
        private IAccountRepository _repository;
        private IDialogService _dialogService;
        private ILogger _logger;

        public AddNewAccountCommand(AccountsViewModel viewModel ,IAccountRepository repository, 
            IDialogService dialogService, ILogger logger)
        {
            _viewModel = viewModel;
            _repository = repository;
            _dialogService = dialogService;
            _logger = logger;
        }

        public override void Execute(object? parameter)
        {
            AddNewAccountViewModel vm = _dialogService.ShowDialog<AddNewAccountViewModel>();

            if (vm != null)
            {
                //_repository.Add(vm.GetModel()); web
                _logger.Information("refreshed");
                _viewModel.AccountsCollectiionView.Refresh();
            }
            else
            {
                _logger.Information("Dialog cancelled (add new account).");
            }
        }
    }
}

using Microsoft.Extensions.Logging;
using SnailPass_Desktop.Model;
using SnailPass_Desktop.ViewModel.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnailPass_Desktop.ViewModel.Commands
{
    internal class AddNewAccountCommand : CommandBase
    {
        private ViewModelBase _viewModel;
        private IAccountRepository _repository;
        private IDialogService _dialogService;
        private ILogger _logger;

        public AddNewAccountCommand(IAccountRepository repository, IDialogService dialogService,
            ILogger logger)
        {
            _repository = repository;
            _dialogService = dialogService;
            _logger = logger;
        }

        public override void Execute(object? parameter)
        {
            AddNewAccountViewModel vm = _dialogService.ShowDialog<AddNewAccountViewModel>();

            if (vm == null)
            {
                _repository.Add(vm.GetModel());
            }
        }
    }
}

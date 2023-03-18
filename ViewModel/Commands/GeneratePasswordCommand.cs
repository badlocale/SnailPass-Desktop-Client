using SnailPass.Model;
using SnailPass.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnailPass.ViewModel.Commands
{
    public class GeneratePasswordCommand : CommandBase
    {
        private AddAccountViewModel _viewModel;
        private IPasswordGenerator _passwordGenerator;

        public GeneratePasswordCommand(AddAccountViewModel viewModel, IPasswordGenerator passwordGenerator)
        {
            _viewModel = viewModel;
            _passwordGenerator = passwordGenerator;
        }

        public override bool CanExecute(object? parameter)
        {
            return _viewModel.IsLowercase || 
                _viewModel.IsUppercase || 
                _viewModel.IsDigits || 
                _viewModel.IsSpecials;
        }

        public override void Execute(object? parameter)
        {
            _viewModel.Password = _passwordGenerator.Generate(_viewModel.Lenght, 
                _viewModel.IsLowercase, 
                _viewModel.IsUppercase, 
                _viewModel.IsDigits, 
                _viewModel.IsSpecials);
        }
    }
}

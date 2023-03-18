using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnailPass.ViewModel.Factories
{
    public interface IViewModelFactory
    {
        public ViewModelBase Create(Type viewModelType);
    }
}
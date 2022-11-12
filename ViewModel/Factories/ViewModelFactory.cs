using Autofac;
using Autofac.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnailPass_Desktop.ViewModel.Factories
{
    public class ViewModelFactory : IViewModelFactory
    {
        private IComponentContext _context;

        public ViewModelFactory(IComponentContext context)
        {
            _context = context;
        }
         
        public ViewModelBase Create(Type viewModelType)
        {
            if (viewModelType.IsAssignableTo(typeof(ViewModelBase)))
            {
                return (ViewModelBase)_context.Resolve(viewModelType);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}

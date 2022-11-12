using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnailPass_Desktop.ViewModel.Services
{
    public interface IDialogService
    {
        public ViewModelBase? ShowDialog(string name, Action<string>? callback = null);
        public TViewModel? ShowDialog<TViewModel>(Action<string>? callback = null)
            where TViewModel : ViewModelBase;
    }
}

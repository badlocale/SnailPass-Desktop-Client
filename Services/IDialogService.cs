using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnailPass_Desktop.ViewModel;

namespace SnailPass_Desktop.Services
{
    public interface IDialogService
    {
        public bool? ShowDialog(string name, Action<string>? callback = null);
        public TViewModel? ShowDialog<TViewModel>(Action<string>? callback = null)
            where TViewModel : ViewModelBase;
    }
}

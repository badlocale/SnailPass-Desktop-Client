using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnailPass.ViewModel.Stores
{
    public interface IApplicationModeStore
    {
        public event EventHandler LocalModeEnabled;
        public event EventHandler LocalModeDisabled;
        bool IsLocalMode { get; }
    }
}

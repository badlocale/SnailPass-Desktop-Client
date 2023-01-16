using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnailPass_Desktop.ViewModel.Stores
{
    public interface IApplicationModeStore
    {
        bool IsLocalMode { get; set; }
    }
}

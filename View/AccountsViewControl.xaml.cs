using SnailPass_Desktop.ViewModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace SnailPass_Desktop.View
{
    public partial class AccountsViewControl : UserControl
    {
        public AccountsViewControl()
        {
            InitializeComponent();
        }

        private void PasswordTextBox_MouseEnter(object sender, MouseEventArgs e)
        {
            AccountsViewModel vm = (AccountsViewModel)DataContext;
            if (vm != null)
            {
                
            }
        }
    }
}

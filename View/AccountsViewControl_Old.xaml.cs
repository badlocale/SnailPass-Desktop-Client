using SnailPass.ViewModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace SnailPass.View
{
    public partial class AccountsViewControl_Old : UserControl
    {
        public AccountsViewControl_Old()
        {
            InitializeComponent();
        }

        private void FieldsListBox_MouseEnter(object sender, MouseEventArgs e)
        {
            ListBox listBox = sender as ListBox;
            listBox?.Focus();
        }
    }
}

using SnailPass.ViewModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace SnailPass.View
{
    public partial class AccountsViewControl : UserControl
    {
        public AccountsViewControl()
        {
            InitializeComponent();
        }

        private void FieldsListBox_MouseEnter(object sender, MouseEventArgs e)
        {
            ListBox listBox = sender as ListBox;
            listBox?.Focus();
        }

        private void FieldsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}

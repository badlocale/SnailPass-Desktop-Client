using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SnailPass.View.Dialogs
{
    [DialogContent]
    public partial class EditCustomFieldDialog : UserControl
    {
        public EditCustomFieldDialog()
        {
            InitializeComponent();
        }

        private void TopMenu_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                (Parent as Window).DragMove();
            }
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            Window window = Parent as Window;

            if (window != null)
            {
                window.DialogResult = true;
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Window window = Parent as Window;

            if (window != null)
            {
                window.DialogResult = false;
            }
        }
    }
}

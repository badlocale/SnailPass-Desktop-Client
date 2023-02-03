using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SnailPass_Desktop.View.Dialogs
{
    [DialogContent]
    public partial class EditAccountDialog : UserControl
    {
        public EditAccountDialog()
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

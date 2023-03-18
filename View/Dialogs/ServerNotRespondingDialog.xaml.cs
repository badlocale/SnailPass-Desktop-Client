using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SnailPass.View.Dialogs
{
    [DialogContent]
    public partial class ServerNotRespondingDialog : UserControl
    {
        public ServerNotRespondingDialog()
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

        private void btnQuit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnContinue_Click(object sender, RoutedEventArgs e)
        {
            Window window = Parent as Window;

            if (window != null)
            {
                window.DialogResult = false;
            }
        }

        private void btnConnent_Click(object sender, RoutedEventArgs e)
        {
            Window window = Parent as Window;

            if (window != null)
            {
                window.DialogResult = true;
            }
        }
    }
}

﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SnailPass.View.Dialogs
{
    [DialogContent]
    public partial class TokenExpiredDialog : UserControl
    {
        public TokenExpiredDialog()
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

        private void BtnDeny_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            Window window = Parent as Window;

            if (window != null)
            {
                window.DialogResult = true;
            }
        }
    }
}

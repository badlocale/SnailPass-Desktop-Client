using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using SnailPass_Desktop.View;

namespace SnailPass_Desktop
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            IntPtr hWnd = new WindowInteropHelper(GetWindow(this)).EnsureHandle();
        }

        private void TopMenu_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void BtnMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                ResizeMode = ResizeMode.CanResize;
                WindowState = WindowState.Normal;
            }
            else
            {
                ResizeMode = ResizeMode.NoResize;
                WindowState = WindowState.Maximized;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace SnailPass.View
{
    public partial class LoginViewContol : UserControl
    {
        public LoginViewContol()
        {
            InitializeComponent();

            TextBlock obj = FindName("SnailPass") as TextBlock;
            Console.WriteLine(obj.FontFamily);
        }
    }
}

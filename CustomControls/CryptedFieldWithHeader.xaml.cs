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

namespace SnailPass_Desktop.CustomControls
{
    public partial class CryptedFieldWithHeader : UserControl
    {
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(CryptedFieldWithHeader), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(string), typeof(CryptedFieldWithHeader), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty HintProperty =
            DependencyProperty.Register("Hint", typeof(string), typeof(CryptedFieldWithHeader), new PropertyMetadata(string.Empty));
        
        public static readonly DependencyProperty DecryptCommandProperty =
            DependencyProperty.Register("DecryptCommand", typeof(ICommand), typeof(CryptedFieldWithHeader));

        public static readonly DependencyPropertyKey IsCopiedPropertyKey =
            DependencyProperty.RegisterReadOnly("IsCopied", typeof(bool), typeof(CryptedFieldWithHeader), new PropertyMetadata(false));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public string Value
        {
            get { return (string)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public string Hint
        {
            get { return (string)GetValue(HintProperty); }
            set { SetValue(HintProperty, value); }
        }

        public ICommand DecryptCommand
        {
            get { return (ICommand)GetValue(DecryptCommandProperty); }
            set { SetValue(DecryptCommandProperty, value); }
        }

        public bool IsCopied
        {
            get 
            {
                return Clipboard.GetText() == Value;
            }
        }

        public CryptedFieldWithHeader()
        {
            InitializeComponent();
        }

        private void CryptedValueField_MouseEnter(object sender, MouseEventArgs e)
        {
            DecryptCommand.Execute(null);
        }

        private void CryptedValueField_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Clipboard.SetText(Value);
        }
    }
}

using System.Windows;
using System.Windows.Controls;

namespace SnailPass_Desktop.CustomControls
{
    public partial class CryptedFieldWithHeader : UserControl
    {
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(CryptedFieldWithHeader), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(string), typeof(CryptedFieldWithHeader), new PropertyMetadata(string.Empty));

        public static readonly DependencyPropertyKey HintProperty =
            DependencyProperty.RegisterReadOnly("Hint", typeof(string), typeof(CryptedFieldWithHeader), new PropertyMetadata(string.Empty));

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

        private string Hint
        {
            get { return (string)GetValue(HintProperty.DependencyProperty); }
            set { SetValue(HintProperty, value); }
        }

        public bool IsCopied
        {
            get { return (bool)GetValue(IsCopiedPropertyKey.DependencyProperty); }
            set { SetValue(IsCopiedPropertyKey, value); }
        }

        public CryptedFieldWithHeader()
        {
            InitializeComponent();
        }

        private void IsCopiedTooltip_ToolTipOpening(object sender, ToolTipEventArgs e)
        {
            IsCopied = Clipboard.GetText() == Value;
        }

        private void CryptedValueField_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Clipboard.SetText(Value);
        }
    }
}

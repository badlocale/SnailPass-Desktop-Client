using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SnailPass_Desktop.CustomControls
{
    public partial class CryptedFieldWithHeader : UserControl
    {
        public static readonly DependencyProperty IsDeletableProperty =
            DependencyProperty.Register("IsDeletable", typeof(bool), typeof(CryptedFieldWithHeader), new PropertyMetadata(true));

        public static readonly DependencyProperty DeleteCommandProperty =
            DependencyProperty.Register("DeleteCommand", typeof(ICommand), typeof(CryptedFieldWithHeader), new PropertyMetadata(null));

        public static readonly DependencyProperty EditCommandProperty =
    DependencyProperty.Register("EditCommand", typeof(ICommand), typeof(CryptedFieldWithHeader), new PropertyMetadata(null));

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(CryptedFieldWithHeader), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(string), typeof(CryptedFieldWithHeader), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty IsButtonsEnabledProperty =
            DependencyProperty.Register("IsButtonsEnabled", typeof(bool), typeof(CryptedFieldWithHeader), new PropertyMetadata(false));

        public static readonly DependencyPropertyKey HintProperty =
            DependencyProperty.RegisterReadOnly("Hint", typeof(string), typeof(CryptedFieldWithHeader), new PropertyMetadata(string.Empty));

        public static readonly DependencyPropertyKey IsCopiedPropertyKey =
            DependencyProperty.RegisterReadOnly("IsCopied", typeof(bool), typeof(CryptedFieldWithHeader), new PropertyMetadata(false));

        public bool IsDeletable
        {
            get { return (bool)GetValue(IsDeletableProperty); }
            set { SetValue(IsDeletableProperty, value); }
        }

        public ICommand DeleteCommand
        {
            get { return (ICommand)GetValue(DeleteCommandProperty); }
            set { SetValue(DeleteCommandProperty, value); }
        }

        public ICommand EditCommand
        {
            get { return (ICommand)GetValue(EditCommandProperty); }
            set { SetValue(EditCommandProperty, value); }
        }

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

        public bool IsButtonsEnabled
        {
            get { return (bool)GetValue(IsButtonsEnabledProperty); }
            set { SetValue(ValueProperty, value); }
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

        private void CryptedValueField_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Clipboard.SetText(Value);
            }
            catch (COMException) { }
        }
    }
}

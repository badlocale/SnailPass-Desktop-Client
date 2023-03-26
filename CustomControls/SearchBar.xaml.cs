using System.Windows;
using System.Windows.Controls;

namespace SnailPass.CustomControls
{
    public partial class SearchBar : UserControl
    {
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(SearchBar), new PropertyMetadata(string.Empty));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public SearchBar()
        {
            InitializeComponent();
        }

        private void ExternalBorder_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            TextBox searchBarTextBox = (TextBox)FindName("SearchBarTextBox");
            searchBarTextBox.Focus();
        }
    }
}

using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using WPControls.Input;

namespace ME.Controls.Test.WP8
{
    public partial class InputView : PhoneApplicationPage
    {
        public InputView()
        {
            InitializeComponent();
        }

        private void TextBlock_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            return;
        }

        private void AdvancedTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var item = sender as AdvancedTextBox;

            if (string.IsNullOrEmpty(item.Text))
            {
                item.VisibleRightButton = Visibility.Collapsed;
            }
            else
            {
                item.VisibleRightButton = Visibility.Visible;
            }
        }
    }
}
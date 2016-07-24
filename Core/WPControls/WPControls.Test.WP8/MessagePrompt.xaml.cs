using System.Windows;
using Microsoft.Phone.Controls;
using System.Windows.Media;
using System.Diagnostics;
using WPControls.Toast;

namespace ME.Controls.Test.WP8
{
    public partial class MessagePrompt : PhoneApplicationPage
    {
        private readonly SolidColorBrush _aliceBlueSolidColorBrush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 240, 248, 255));
        private readonly SolidColorBrush _naturalBlueSolidColorBrush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 0, 135, 189));
        private readonly SolidColorBrush _cornFlowerBlueSolidColorBrush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(200, 100, 149, 237));

        const string LongText = "Testing text body wrapping with a bit of Lorem Ipsum.  Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed at orci felis, in imperdiet tortor.";

        private WPControls.Toast.MessagePrompt _prompt;
        private ConfirmationPrompt _confirmationPrompt;

        public MessagePrompt()
        {
            InitializeComponent();
        }

        private void BasicMessage_Click(object sender, RoutedEventArgs e)
        {
            _prompt = new WPControls.Toast.MessagePrompt();

            _prompt.Title = "Basic Message";
            _prompt.Message = LongText;
            _prompt.Overlay = _cornFlowerBlueSolidColorBrush;
            _prompt.CancelButtonMessage = "Cancel";
            _prompt.Background = new SolidColorBrush(Colors.White);
            _prompt.Foreground = new SolidColorBrush(Colors.Black);
            _prompt.Margin = new Thickness(10, 10, 10, 10);
            _prompt.CancelButtonMouseOverBrush = _naturalBlueSolidColorBrush;
            _prompt.CornerRadius = new CornerRadius(10);
            _prompt.OnCancelClick += _prompt_OnCancelClick;

            _prompt.Show();
        }

        private void _prompt_OnCancelClick()
        {
            Debug.WriteLine("Cancel Message Prompt");
        }

        private void Confirmation_Click(object sender, RoutedEventArgs e)
        {
            _confirmationPrompt = new ConfirmationPrompt();

            _confirmationPrompt.Title = "Basic Message";
            _confirmationPrompt.Message = LongText;
            _confirmationPrompt.Overlay = _cornFlowerBlueSolidColorBrush;
            _confirmationPrompt.RightButtonMessage = "Cancel";
            _confirmationPrompt.LeftButtonMessage = "Ok";
            _confirmationPrompt.Background = new SolidColorBrush(Colors.White);
            _confirmationPrompt.Foreground = new SolidColorBrush(Colors.Black);
            _confirmationPrompt.Margin = new Thickness(10, 10, 10, 10);
            _confirmationPrompt.ActionButtonMouseOverBrush = _naturalBlueSolidColorBrush;
            _confirmationPrompt.CornerRadius = new CornerRadius(10);
            _confirmationPrompt.RightButtonForeground = new SolidColorBrush(Colors.Cyan);
            _confirmationPrompt.OnLeftClick += _confirmationPrompt_OnLeftClick;
            _confirmationPrompt.OnRightClick += _confirmationPrompt_OnRightClick;

            _confirmationPrompt.Show();
        }

        private void _confirmationPrompt_OnRightClick()
        {
            Debug.WriteLine("Right clicked");
        }

        private void _confirmationPrompt_OnLeftClick()
        {
            Debug.WriteLine("Left clicked");
        }
    }
}
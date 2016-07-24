using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WPControls.Toast;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace ME.Controls.Test.WP81
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MessagePromptView : Page
    {
        private readonly SolidColorBrush _aliceBlueSolidColorBrush = new SolidColorBrush(Color.FromArgb(255, 240, 248, 255));
        private readonly SolidColorBrush _naturalBlueSolidColorBrush = new SolidColorBrush(Color.FromArgb(255, 0, 135, 189));
        private readonly SolidColorBrush _cornFlowerBlueSolidColorBrush = new SolidColorBrush(Color.FromArgb(200, 100, 149, 237));

        const string LongText = "Testing text body wrapping with a bit of Lorem Ipsum.  Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed at orci felis, in imperdiet tortor.";

        private MessagePrompt _prompt;
        private ConfirmationPrompt _confirmationPrompt;
        private CustomPrompt _customPrompt;

        public MessagePromptView()
        {
            InitializeComponent();
        }

        private void BasicMessage_Click(object sender, RoutedEventArgs e)
        {
            _prompt = new MessagePrompt();

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

        private void Custom_Click(object sender, RoutedEventArgs e)
        {
            _customPrompt = new CustomPrompt();
            _customPrompt.Background = new SolidColorBrush(Colors.White);
            _customPrompt.Overlay = _cornFlowerBlueSolidColorBrush;
            _customPrompt.Content = new TestCustomUC();
            _customPrompt.Show();
            //_customPrompt.SetContent(this);
        }
    }
}

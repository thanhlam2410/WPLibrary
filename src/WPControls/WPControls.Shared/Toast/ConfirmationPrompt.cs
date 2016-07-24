using System;
#if WINDOWS_STORE || WINDOWS_PHONE_APP
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;
using Windows.UI.Core;
using Windows.UI.Xaml.Input;
using Windows.UI;
#elif WINDOWS_PHONE
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
#endif

namespace WPControls.Toast
{
    public class ConfirmationPrompt: PopUp<string, PopUpResult>
    {
        #region Fields
        private Border _leftActionBorder;
        private Border _rightActionBorder;
        #endregion

        #region Events
        public event Action OnLeftClick;
        public event Action OnRightClick;
        #endregion

        #region Methods
        public ConfirmationPrompt()
        {
            DefaultStyleKey = typeof(ConfirmationPrompt);
        }

        protected internal void SetBodyMessage()
        {
            Body = new TextBlock
            {
                Text = Message,
                TextWrapping = TextWrapping.Wrap
            };
        }

#if WINDOWS_STORE || WINDOWS_PHONE_APP
        protected override void OnApplyTemplate()
#elif WINDOWS_PHONE
		public override void OnApplyTemplate()
#endif
        {
            base.OnApplyTemplate();
            _leftActionBorder = GetTemplateChild("LeftActionBorder") as Border;
            _rightActionBorder = GetTemplateChild("RightActionBorder") as Border;

#if WINDOWS_STORE || WINDOWS_PHONE_APP
            if (_leftActionBorder != null)
            {
                _leftActionBorder.Tapped += _leftActionBorder_Click;
                _leftActionBorder.PointerEntered += _cancelBorder_Enter;
                _leftActionBorder.PointerExited += _cancelBorder_Leave;
            }

            if (_rightActionBorder != null)
            {
                _rightActionBorder.Tapped += _rightActionBorder_Click;
                _rightActionBorder.PointerEntered += _cancelBorder_Enter;
                _rightActionBorder.PointerExited += _cancelBorder_Leave;
            }

            Focus(FocusState.Programmatic);
#else
            if (_leftActionBorder != null)
            {
                _leftActionBorder.Tap += _leftActionBorder_Click;
                _leftActionBorder.MouseEnter += _cancelBorder_Enter;
                _leftActionBorder.MouseLeave += _cancelBorder_Leave;
            }

            if (_rightActionBorder != null)
            {
                _rightActionBorder.Tap += _rightActionBorder_Click;
                _rightActionBorder.MouseEnter += _cancelBorder_Enter;
                _rightActionBorder.MouseLeave += _cancelBorder_Leave;
            }

            Focus();
#endif
        }

#if WINDOWS_STORE || WINDOWS_PHONE_APP
        private void _cancelBorder_Leave(object sender, PointerRoutedEventArgs e)
#else
        private void _cancelBorder_Leave(object sender, MouseEventArgs e)
#endif
        {
            var border = sender as Border;
            border.Background = new SolidColorBrush(Colors.Transparent);
        }

#if WINDOWS_STORE || WINDOWS_PHONE_APP
        private void _cancelBorder_Enter(object sender, PointerRoutedEventArgs e)
#else
        private void _cancelBorder_Enter(object sender, MouseEventArgs e)
#endif
        {
            var border = sender as Border;
            border.Background = ActionButtonMouseOverBrush;
        }

#if WINDOWS_STORE || WINDOWS_PHONE_APP
        private void _leftActionBorder_Click(object sender, TappedRoutedEventArgs e)
#else
        private void _leftActionBorder_Click(object sender, GestureEventArgs e)
#endif

        {
            Hide();

            if (OnLeftClick != null)
            {
                OnLeftClick.Invoke();
            }
        }

#if WINDOWS_STORE || WINDOWS_PHONE_APP
        private void _rightActionBorder_Click(object sender, TappedRoutedEventArgs e)
#else
        private void _rightActionBorder_Click(object sender, GestureEventArgs e)
#endif

        {
            Hide();

            if (OnRightClick != null)
            {
                OnRightClick.Invoke();
            }
        }
        #endregion

        #region Dependency Property Callbacks
        private static void OnMesageContentChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var sender = obj as ConfirmationPrompt;

            if (sender != null)
            {
                sender.SetBodyMessage();
            }
        }
        #endregion

        #region Dependency Properties / Properties
        public object Body
        {
            get { return GetValue(BodyProperty); }
            set { SetValue(BodyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Body.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BodyProperty =
            DependencyProperty.Register("Body", typeof(object), typeof(ConfirmationPrompt), new PropertyMetadata(null));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(ConfirmationPrompt), new PropertyMetadata(""));

        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Message.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(ConfirmationPrompt), new PropertyMetadata("", OnMesageContentChanged));

        public string LeftButtonMessage
        {
            get { return (string)GetValue(LeftButtonMessageProperty); }
            set { SetValue(LeftButtonMessageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CancelButtonMessage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LeftButtonMessageProperty =
            DependencyProperty.Register("LeftButtonMessage", typeof(string), typeof(ConfirmationPrompt), new PropertyMetadata(""));

        public string RightButtonMessage
        {
            get { return (string)GetValue(RightButtonMessageProperty); }
            set { SetValue(RightButtonMessageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RightButtonMessage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RightButtonMessageProperty =
            DependencyProperty.Register("RightButtonMessage", typeof(string), typeof(ConfirmationPrompt), new PropertyMetadata(""));
        
        public SolidColorBrush LeftButtonForeground
        {
            get { return (SolidColorBrush)GetValue(LeftButtonForegroundProperty); }
            set { SetValue(LeftButtonForegroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LeftButtonForeground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LeftButtonForegroundProperty =
            DependencyProperty.Register("LeftButtonForeground", typeof(SolidColorBrush), typeof(ConfirmationPrompt), new PropertyMetadata(new SolidColorBrush(Colors.Black)));

        public SolidColorBrush RightButtonForeground
        {
            get { return (SolidColorBrush)GetValue(RightButtonForegroundProperty); }
            set { SetValue(RightButtonForegroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RightButtonForeground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RightButtonForegroundProperty =
            DependencyProperty.Register("RightButtonForeground", typeof(SolidColorBrush), typeof(ConfirmationPrompt), new PropertyMetadata(new SolidColorBrush(Colors.Black)));

        public SolidColorBrush ActionButtonMouseOverBrush
        {
            get { return (SolidColorBrush)GetValue(ActionButtonMouseOverBrushProperty); }
            set { SetValue(ActionButtonMouseOverBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CancelButtonMouseOverBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActionButtonMouseOverBrushProperty =
            DependencyProperty.Register("ActionButtonMouseOverBrush", typeof(SolidColorBrush), typeof(ConfirmationPrompt), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CornerRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(ConfirmationPrompt), new PropertyMetadata(new CornerRadius(0)));
        #endregion
    }
}

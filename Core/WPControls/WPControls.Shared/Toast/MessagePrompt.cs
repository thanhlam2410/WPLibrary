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
    public class MessagePrompt : PopUp<string, PopUpResult>
    {
        #region Fields
        private Border _cancelBorder;
        #endregion

        #region Events
        public event Action OnCancelClick;
        #endregion

        #region Methods
        public MessagePrompt()
        {
            DefaultStyleKey = typeof(MessagePrompt);
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
            _cancelBorder = GetTemplateChild("CancelBorder") as Border;

#if WINDOWS_STORE || WINDOWS_PHONE_APP
            if (_cancelBorder != null)
            {
                _cancelBorder.Tapped += _cancelBorder_Tapped;
                _cancelBorder.PointerEntered += _cancelBorder_Enter;
                _cancelBorder.PointerExited += _cancelBorder_Leave;
            }
            
            Focus(FocusState.Programmatic);
#else
            if (_cancelBorder != null)
            {
                _cancelBorder.Tap += _cancelBorder_Tap;
                _cancelBorder.MouseEnter += _cancelBorder_Enter;
                _cancelBorder.MouseLeave += _cancelBorder_Leave;
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
            _cancelBorder.Background = new SolidColorBrush(Colors.Transparent);
        }

#if WINDOWS_STORE || WINDOWS_PHONE_APP
        private void _cancelBorder_Enter(object sender, PointerRoutedEventArgs e)
#else
        private void _cancelBorder_Enter(object sender, MouseEventArgs e)
#endif
        {
            _cancelBorder.Background = CancelButtonMouseOverBrush;
        }

#if WINDOWS_STORE || WINDOWS_PHONE_APP
        private void _cancelBorder_Tapped(object sender, TappedRoutedEventArgs e)
#else
        private void _cancelBorder_Tap(object sender, GestureEventArgs e)
#endif

        {
            Hide();

            if (OnCancelClick != null)
            {
                OnCancelClick.Invoke();
            }
        }
        #endregion

        #region Dependency Property Callbacks
        private static void OnMesageContentChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var sender = obj as MessagePrompt;

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
            DependencyProperty.Register("Body", typeof(object), typeof(MessagePrompt), new PropertyMetadata(null));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(MessagePrompt), new PropertyMetadata(""));

        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Message.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(MessagePrompt), new PropertyMetadata("", OnMesageContentChanged));

        public string CancelButtonMessage
        {
            get { return (string)GetValue(CancelButtonMessageProperty); }
            set { SetValue(CancelButtonMessageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CancelButtonMessage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CancelButtonMessageProperty =
            DependencyProperty.Register("CancelButtonMessage", typeof(string), typeof(MessagePrompt), new PropertyMetadata(""));

        public SolidColorBrush CancelButtonForeground
        {
            get { return (SolidColorBrush)GetValue(CancelButtonForegroundProperty); }
            set { SetValue(CancelButtonForegroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CancelButtonForeground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CancelButtonForegroundProperty =
            DependencyProperty.Register("CancelButtonForeground", typeof(SolidColorBrush), typeof(MessagePrompt), new PropertyMetadata(new SolidColorBrush(Colors.Black)));

        public SolidColorBrush CancelButtonMouseOverBrush
        {
            get { return (SolidColorBrush)GetValue(CancelButtonMouseOverBrushProperty); }
            set { SetValue(CancelButtonMouseOverBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CancelButtonMouseOverBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CancelButtonMouseOverBrushProperty =
            DependencyProperty.Register("CancelButtonMouseOverBrush", typeof(SolidColorBrush), typeof(MessagePrompt), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CornerRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(MessagePrompt), new PropertyMetadata(new CornerRadius(0)));
        #endregion
    }
}

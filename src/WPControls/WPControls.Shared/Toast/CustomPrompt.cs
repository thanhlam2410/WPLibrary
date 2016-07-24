using System;
using System.Collections.Generic;
using System.Text;


#if WINDOWS_STORE || WINDOWS_PHONE_APP
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
#elif WINDOWS_PHONE
using System.Windows;
using System.Windows.Controls;
#endif

namespace WPControls.Toast
{
    public class CustomPrompt: PopUp<string, PopUpResult>
    {
#region Fields
        private Panel _bodyContainer;
#endregion

#region Methods
        public CustomPrompt()
        {
            DefaultStyleKey = typeof(CustomPrompt);
        }

#if WINDOWS_STORE || WINDOWS_PHONE_APP
        protected override void OnApplyTemplate()
#elif WINDOWS_PHONE
        public override void OnApplyTemplate()
#endif
        {
            base.OnApplyTemplate();
            _bodyContainer = GetTemplateChild("BodyContainer") as Panel;

            if (_bodyContainer != null)
            {
                var body = Content as UIElement;

                if (body != null)
                {
                    _bodyContainer.Children.Add(body);
                }
            }

#if WINDOWS_STORE || WINDOWS_PHONE_APP
            Focus(FocusState.Programmatic);
#else
            Focus();
#endif
        }
        #endregion

        public override void Hide()
        {
            _bodyContainer.Children.Clear();
            base.Hide();
        }

        #region Dependency Properties / Properties
        public object Content
        {
            get { return (object)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Content.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(object), typeof(CustomPrompt), new PropertyMetadata(null));
        #endregion
    }
}

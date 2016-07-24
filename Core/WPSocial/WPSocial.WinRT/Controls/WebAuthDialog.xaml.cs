using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WPSocial.Enums;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace WPSocial.Controls
{
    public sealed partial class WebAuthDialog : UserControl
    {
        #region Fields
        private ResponseTypes responseType;
        #endregion

        #region Events
        public event Action<string> OnAuthenticating;
        public event Action OnCanceled;
        #endregion

        public WebAuthDialog()
        {
            this.InitializeComponent();
            this.Loaded += WebAuthDialog_Loaded;
        }

        private void WebAuthDialog_Loaded(object sender, RoutedEventArgs e)
        {
            this.Width = Window.Current.Bounds.Width;
            this.Height = Window.Current.Bounds.Height;
        }

        private void dialogWebBrowser_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            loadingRing.Visibility = Visibility.Visible;

            if (OnAuthenticating != null && responseType == ResponseTypes.Url)
            {
                OnAuthenticating.Invoke(args.Uri.OriginalString);
            }
        }

        private void closeDialogButton_Click(object sender, RoutedEventArgs e)
        {
            if (OnCanceled != null)
            {
                OnCanceled.Invoke();
            }
        }

        public void OpenWeb(string url, ResponseTypes type)
        {
            dialogWebBrowser.Navigate(new Uri(url, UriKind.RelativeOrAbsolute));
            loadingRing.Visibility = Visibility.Visible;

            responseType = type;
        }

        private async void dialogWebBrowser_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            loadingRing.Visibility = Visibility.Collapsed;
            if (OnAuthenticating != null && responseType == ResponseTypes.Title)
            {
                string[] argsWeb = { "document.title;" };
                string title = await dialogWebBrowser.InvokeScriptAsync("eval", argsWeb);

                OnAuthenticating.Invoke(title);
            }
        }
    }
}

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using WPSocial.Enums;

namespace WPSocial.Controls
{
    public partial class WebAuthDialog : UserControl
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
            InitializeComponent();
            this.Loaded += WebAuthDialog_Loaded;
        }

        private void WebAuthDialog_Loaded(object sender, RoutedEventArgs e)
        {
            this.Width = Application.Current.Host.Content.ActualWidth;
            this.Height = Application.Current.Host.Content.ActualHeight;
        }

        private void dialogWebBrowser_Navigating(object sender, NavigatingEventArgs e)
        {
            loadingRing.Visibility = Visibility.Visible;
            if (OnAuthenticating != null && responseType == ResponseTypes.Url)
            {
                OnAuthenticating.Invoke(e.Uri.OriginalString);
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

        public void ClearCookie()
        {
            dialogWebBrowser.ClearCookiesAsync();
        }

        private void dialogWebBrowser_Navigated(object sender, NavigationEventArgs e)
        {
            loadingRing.Visibility = Visibility.Collapsed;
            if (OnAuthenticating != null && responseType == ResponseTypes.Title)
            {
                string title = (string)dialogWebBrowser.InvokeScript("eval", "document.title.toString()");
                OnAuthenticating.Invoke(title);
            }
        }
    }
}

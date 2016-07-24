using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using WPSocial.Enums;

namespace WPSocial.Controls
{
    internal class WebAuthenticationBroker
    {
        #region Fields
        private Popup mainPopup;
        private SupportedPageOrientation appDisplayOrientation;

        private Func<string, bool> validateResponseFunc;
        #endregion

        #region Events
        public event Action<string> OnSuccess;
        public event Action OnClosed;
        #endregion

        #region Methods
        public WebAuthenticationBroker()
        {
            mainPopup = new Popup();
            mainPopup.VerticalAlignment = VerticalAlignment.Stretch;
            mainPopup.HorizontalAlignment = HorizontalAlignment.Stretch;
        }

        public void Authenticate(string authUri, ResponseTypes responseType, Func<string, bool> validateFunc, bool requireCacheCleared = true)
        {
            WebAuthDialog authenticationDialog = new WebAuthDialog();
            authenticationDialog.OnAuthenticating += AuthenticationDialog_OnAuthenticating;
            authenticationDialog.OnCanceled += AuthenticationDialog_OnCanceled;

            mainPopup.Child = authenticationDialog;
            mainPopup.IsOpen = true;

            SystemTray.IsVisible = false;

            var currentPage = ((PhoneApplicationFrame)Application.Current.RootVisual).Content as PhoneApplicationPage;
            appDisplayOrientation = currentPage.SupportedOrientations;
            currentPage.SupportedOrientations = SupportedPageOrientation.Portrait;

            validateResponseFunc = validateFunc;

            if (requireCacheCleared)
            {
                authenticationDialog.ClearCookie();
            }
            
            authenticationDialog.OpenWeb(authUri, responseType);
        }

        private void AuthenticationDialog_OnCanceled()
        {
            Close();

            if (OnClosed != null)
            {
                OnClosed.Invoke();
            }
        }

        private void AuthenticationDialog_OnAuthenticating(string uri)
        {
            if (validateResponseFunc == null)
            {
                return;
            }

            if (validateResponseFunc(uri))
            {
                Close();

                if (OnSuccess != null)
                {
                    OnSuccess.Invoke(uri);
                }
            }
        }

        private void Close()
        {
            mainPopup.IsOpen = false;
            mainPopup.Child = null;

            var currentPage = ((PhoneApplicationFrame)Application.Current.RootVisual).Content as PhoneApplicationPage;
            currentPage.SupportedOrientations = appDisplayOrientation;
        }
        #endregion
    }
}

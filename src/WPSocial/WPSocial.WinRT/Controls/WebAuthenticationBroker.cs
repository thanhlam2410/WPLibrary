using System;
using Windows.Graphics.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using WPSocial.Enums;

namespace WPSocial.Controls
{
    internal class WebAuthenticationBroker
    {
        #region Fields
        private Popup mainPopup;
        private DisplayOrientations appDisplayOrientation;

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
            mainPopup.IsLightDismissEnabled = false;
        }

        public void Authenticate(string authUri, ResponseTypes responseType, Func<string, bool> validateFunc, bool requireCacheCleared = true)
        {
            WebAuthDialog authenticationDialog = new WebAuthDialog();
            authenticationDialog.OnAuthenticating += AuthenticationDialog_OnAuthenticating;
            authenticationDialog.OnCanceled += AuthenticationDialog_OnCanceled;

            mainPopup.Child = authenticationDialog;
            mainPopup.IsOpen = true;

            if (requireCacheCleared)
            {
                Uri cacheUri = new Uri(authUri, UriKind.RelativeOrAbsolute);
                string baseUri = authUri.Replace(cacheUri.PathAndQuery, "");

                ClearBrowserCookie(baseUri);
            }
            
            validateResponseFunc = validateFunc;
            
            appDisplayOrientation = DisplayInformation.AutoRotationPreferences;
            DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait;
            
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
            if (validateResponseFunc(uri))
            {
                Close();

                if (OnSuccess != null)
                {
                    OnSuccess.Invoke(uri);
                }
            }
        }

        private void ClearBrowserCookie(string baseUri)
        {
            if (!string.IsNullOrEmpty(baseUri))
            {
                Windows.Web.Http.Filters.HttpBaseProtocolFilter myFilter = new Windows.Web.Http.Filters.HttpBaseProtocolFilter();
                var cookieManager = myFilter.CookieManager;
                Windows.Web.Http.HttpCookieCollection myCookieJar = cookieManager.GetCookies(new Uri(baseUri, UriKind.RelativeOrAbsolute));
                foreach (Windows.Web.Http.HttpCookie cookie in myCookieJar)
                {
                    cookieManager.DeleteCookie(cookie);
                }
            }
        }

        private void Close()
        {
            mainPopup.IsOpen = false;
            mainPopup.Child = null;

            DisplayInformation.AutoRotationPreferences = appDisplayOrientation;
        }
        #endregion
    }
}

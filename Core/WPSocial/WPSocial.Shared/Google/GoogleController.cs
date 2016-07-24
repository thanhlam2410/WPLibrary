using System;
using System.Diagnostics;
using System.Threading.Tasks;
using WPCore.Storage;
using WPSocial.Controls;
using WPSocial.Enums;
using WPSocial.Google.Models.Data;

namespace WPSocial.Google
{
    internal class GoogleController
    {
        #region Event
        private Action<GoogleOauthResult> loginCallback;
        #endregion

        #region Fields
        private GoogleAPI googleApi;
        private StorageHelper storageService;
        #endregion

        #region Methods
        public GoogleController()
        {
            googleApi = new GoogleAPI();
            storageService = new StorageHelper();
        }

        #region Authentication
        public async void Authenticate(string scope, Action<GoogleOauthResult> callback)
        {
            loginCallback = callback;

            ModelGoogleAuthorization oauthModel = await RestoreAccessToken();

            if (oauthModel != null)
            {
                GoogleOauthResult result = new GoogleOauthResult();

                result.AccessToken = oauthModel.AccessToken;
                result.RefreshToken = oauthModel.RefreshToken;
                result.ExpiredIn = oauthModel.ExpiredIn;
                result.TokenType = oauthModel.TokenType;

                if (loginCallback != null)
                {
                    loginCallback.Invoke(result);
                }
            }
            else
            {
                string uri = string.Format(GoogleConstants.GOOGLE_AUTH_FORMAT,
                   GoogleConstants.GOOGLE_AUTH_URL,
                   GoogleModel.Instance.ClientId,
                   GoogleConstants.GOOGLE_AUTH_REDIRECT_URI,
                   scope);

                WebAuthenticationBroker authenticationBroker = new WebAuthenticationBroker();
                authenticationBroker.OnSuccess += AuthenticationBroker_OnSuccess;

                Func<string, bool> validateMethod = (arg) =>
                {
                    return (arg.StartsWith("Success") || arg.StartsWith("Denied"));
                };

                authenticationBroker.Authenticate(uri, ResponseTypes.Title, validateMethod);
            }
        }
        
        /// <summary>
        /// Clear authentication cache
        /// </summary>
        public void Logout()
        {
            storageService.Delete(GoogleConstants.GOOGLE_STORE_REFRESH_TOKEN + GoogleModel.Instance.ClientId);
        }

        /// <summary>
        /// Handle authentication callback
        /// </summary>
        /// <param name="arg"></param>
        private async void AuthenticationBroker_OnSuccess(string arg)
        {
            Debug.WriteLine(arg);
            GoogleOauthResult result = new GoogleOauthResult();

            if (arg.StartsWith("Success"))
            {
                result.IsSuccess = true;
                string authorizationCode = arg.Substring(arg.IndexOf('=') + 1);

                ModelGoogleAuthorization model = await googleApi.GetAccessToken(authorizationCode, GoogleModel.Instance.ClientId, GoogleModel.Instance.ClientSecret);

                if (model != null)
                {
                    result.AccessToken = model.AccessToken;
                    result.RefreshToken = model.RefreshToken;
                    result.ExpiredIn = model.ExpiredIn;
                    result.TokenType = model.TokenType;

                    SaveRefreshToken(model.RefreshToken);
                }
            }
            else
            {
                string error = arg.Substring(arg.IndexOf('=') + 1);

                result.IsSuccess = false;
                result.Error = error;
            }

            if (loginCallback != null)
            {
                loginCallback.Invoke(result);
            }
        }

        /// <summary>
        /// Store refresh token for later use
        /// </summary>
        /// <param name="refreshToken"></param>
        private void SaveRefreshToken(string refreshToken)
        {
            string key = GoogleConstants.GOOGLE_STORE_REFRESH_TOKEN + GoogleModel.Instance.ClientId;
            storageService.Save(key, refreshToken);
        }

        /// <summary>
        /// Restore access token if refresh token available
        /// </summary>
        /// <returns></returns>
        private async Task<ModelGoogleAuthorization> RestoreAccessToken()
        {
            string key = GoogleConstants.GOOGLE_STORE_REFRESH_TOKEN + GoogleModel.Instance.ClientId;

            if (storageService.IsExist(key))
            {
                string refreshToken = storageService.Load(key).ToString();

                if (!string.IsNullOrEmpty(refreshToken))
                {
                    return await googleApi.RefreshAccessToken(refreshToken);
                }
            }

            return null;
        }
        #endregion
        #endregion
    }
}

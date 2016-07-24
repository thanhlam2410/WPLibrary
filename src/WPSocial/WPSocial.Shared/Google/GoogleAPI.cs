using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using WPCore.Http;
using WPCore.Http.Enums;
using WPCore.Http.Models;
using WPCore.Json;
using WPSocial.Delegates;
using WPSocial.Google.Models.Data;

namespace WPSocial.Google
{
    internal class GoogleAPI
    {
        #region Fields
        private AdvancedHttpClient httpClient;
        #endregion

        #region Events
        public event HttpFailedHandler OnFailed;
        #endregion

        #region Constants
        private const string GOOGLE_TOKEN_URL = @"https://www.googleapis.com/oauth2/v3/token";
        private const string GOOGLE_PLUS_URL = @"https://www.googleapis.com/plus/v1/people/";
        #endregion

        #region Methods
        public GoogleAPI()
        {
            httpClient = new AdvancedHttpClient();
        }

        private async Task<string> RunAsync(string domain, Dictionary<string, string> data, [CallerMemberName]string tag = "")
        {
            FormDataRequestModel request = new FormDataRequestModel()
            {
                Data = data,
                Domain = domain,
                Method = HttpMethods.POST,
                Tag = tag,
                Timeout = 30
            };

            HttpResultModel result = await httpClient.SendRequest(request);

            if (result.IsSuccess)
            {
                return result.ResposeData;
            }
            else
            {
                if (OnFailed != null)
                {
                    OnFailed.Invoke(result.ResponseCode, result.ErrorMessage, result.Tag);
                }

                return string.Empty;
            }
        }

        #region Access Token
        public async Task<ModelGoogleAuthorization> GetAccessToken(string authorizationCode, string clientId, string clientSecret)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();

            data.Add("code", authorizationCode);
            data.Add("client_id", clientId);
            data.Add("client_secret", clientSecret);
            data.Add("redirect_uri", @"urn:ietf:wg:oauth:2.0:oob:auto");
            data.Add("grant_type", "authorization_code");

            string response = await RunAsync(GOOGLE_TOKEN_URL, data);

            if (!string.IsNullOrEmpty(response))
            {
                return JsonConverter.GetObject<ModelGoogleAuthorization>(response);
            }

            return null;
        }

        /// <summary>
        /// Restore google access token
        /// </summary>
        /// <returns></returns>
        public async Task<ModelGoogleAuthorization> RefreshAccessToken(string refreshToken)
        {
            if (!string.IsNullOrEmpty(GoogleModel.Instance.RefreshToken))
            {
                Dictionary<string, string> data = new Dictionary<string, string>();

                data.Add("client_id", GoogleModel.Instance.ClientId);
                data.Add("client_secret", GoogleModel.Instance.ClientSecret);
                data.Add("refresh_token", GoogleModel.Instance.RefreshToken);
                data.Add("grant_type", "refresh_token");

                string response = await RunAsync(GOOGLE_TOKEN_URL, data);

                if (!string.IsNullOrEmpty(response))
                {
                    return JsonConverter.GetObject<ModelGoogleAuthorization>(response);
                }
            }

            return null;
        }
        #endregion

        #region User
        /// <summary>
        /// get user's google profile
        /// </summary>
        public async Task<GoogleProfile> GetGoogleProfile(string userId = "me")
        {
            string accessToken = GoogleModel.Instance.AccessToken;

            if (!string.IsNullOrEmpty(accessToken))
            {
                string domain = GOOGLE_PLUS_URL + userId;
                string fields = "displayName,image/url";

                Dictionary<string, string> data = new Dictionary<string, string>();

                data.Add("fields", fields);
                data.Add("prettyPrint", "false");
                data.Add("access_token", accessToken);

                string response = await RunAsync(domain, data);

                if (!string.IsNullOrEmpty(response))
                {
                    return JsonConverter.GetObject<GoogleProfile>(response);
                }
            }

            return null;
        }
        #endregion
        #endregion
    }
}

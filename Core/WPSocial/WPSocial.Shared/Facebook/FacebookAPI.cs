using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using WPCore.Http;
using WPCore.Http.Enums;
using WPCore.Http.Models;
using WPCore.Json;
using WPSocial.Delegates;
using WPSocial.Facebook.Models.Data;

namespace WPSocial.Facebook
{
    public class FacebookAPI
    {
        #region Fields
        private AdvancedHttpClient httpClient;
        #endregion

        #region Events
        public event HttpFailedHandler OnFailed;
        #endregion

        #region Constants
        private const string FACEBOOK_GRAPH = @"https://graph.facebook.com/";
        #endregion

        #region Methods
        public FacebookAPI()
        {
            httpClient = new AdvancedHttpClient();
        }

        private async Task<string> RunAsync(string domain, Dictionary<string, string> data, [CallerMemberName]string tag = "")
        {
            FormDataRequestModel request = new FormDataRequestModel()
            {
                Data = data,
                Domain = domain,
                Method = HttpMethods.GET,
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

        /// <summary>
        /// Get facebook user info
        /// </summary>
        /// <param name="facebookId"></param>
        /// <returns></returns>
        public async Task<FacebookUserInfo> GetFacebookInfo(string facebookId, string requireFields)
        {
            string accessToken = FacebookModel.Instance.FacebookAccessToken;

            if (!string.IsNullOrEmpty(accessToken))
            {
                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("access_token", accessToken);
                data.Add("fields", requireFields);

                string domain = FACEBOOK_GRAPH + facebookId;

                string response = await RunAsync(domain, data);

                if (!string.IsNullOrEmpty(response))
                {
                    return JsonConverter.GetObject<FacebookUserInfo>(response);
                }
            }

            return null;
        }

        /// <summary>
        /// Get facebook avatar
        /// </summary>
        /// <param name="facebookID"></param>
        /// <returns></returns>
        public string GetFacebookAvatar(string facebookID)
        {
            if (string.IsNullOrEmpty(FacebookModel.Instance.FacebookAccessToken))
            {
                return string.Empty;
            }

            return FACEBOOK_GRAPH + facebookID + "/picture?access_token=" + FacebookModel.Instance.FacebookAccessToken;
        }
        #endregion
    }
}

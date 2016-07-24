using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using WPCore.Platform;
using WPCore.Storage;
using WPSocial.Controls;
using WPSocial.Enums;
using WPSocial.Facebook.Exceptions;
using WPSocial.Facebook.Models;
using WPSocial.Facebook.Models.Data;

namespace WPSocial.Facebook
{
    internal class FacebookController
    {
        #region Fields
        private Action<FacebookOauthResult> loginCallback;
        private Action<FacebookActionResult> actionCallback;
        private StorageHelper storageService;
        private FacebookAPI facebookAPI;
        #endregion

        #region Methods
        public FacebookController()
        {
            storageService = new StorageHelper();
            facebookAPI = new FacebookAPI();

            facebookAPI.OnFailed += FacebookAPI_OnFailed;
        }

        #region Authentication
        /// <summary>
        /// Authenticate facebook user
        /// </summary>
        /// <param name="permissions"></param>
        /// <param name="callback"></param>
        public void Authenticate(List<string> permissions, Action<FacebookOauthResult> callback)
        {
            if (string.IsNullOrEmpty(FacebookModel.Instance.AppId))
            {
                throw new FacebookOauthException("AppId must not be empty");
            }

            loginCallback = callback;

            //try to restore local access token
            string accessToken = RestoreAccessToken();

            if (!string.IsNullOrEmpty(accessToken))
            {
                if (loginCallback != null)
                {
                    FacebookOauthResult result = new FacebookOauthResult();
                    result.IsSuccess = true;
                    result.AccessToken = accessToken;

                    loginCallback.Invoke(result);
                }

                return;
            }

            //promt oauth dialog
            string appId = FacebookModel.Instance.AppId;
            string scope = ConstructPermission(permissions);

            string loginUri = string.Format(FacebookConstants.FACEBOOK_AUTH_URL_FORMAT,
                appId,
                FacebookConstants.FACEBOOK_REDIRECT_URI);

            if (!string.IsNullOrEmpty(scope))
            {
                loginUri += string.Format("&scope={0}", scope);
            }

            WebAuthenticationBroker authenticationBroker = new WebAuthenticationBroker();
            authenticationBroker.OnSuccess += AuthenticationBroker_OnSuccess;

            Func<string, bool> validateMethod = (arg) =>
            {
                return !string.IsNullOrEmpty(arg) && arg.StartsWith(FacebookConstants.FACEBOOK_REDIRECT_URI);
            };

            authenticationBroker.Authenticate(loginUri, ResponseTypes.Url, validateMethod);
        }

        /// <summary>
        /// Clear all user's authentication cache
        /// </summary>
        public void Logout()
        {
            storageService.Delete(FacebookConstants.FACEBOOK_STORE_ACCESS_TOKEN + FacebookModel.Instance.AppId);
        }

        /// <summary>
        /// Restore local access token
        /// </summary>
        /// <returns></returns>
        private string RestoreAccessToken()
        {
            string key = FacebookConstants.FACEBOOK_STORE_ACCESS_TOKEN + FacebookModel.Instance.AppId;

            if (storageService.IsExist(key))
            {
                string accessToken = storageService.Load(key).ToString();
                FacebookModel.Instance.FacebookAccessToken = accessToken;

                return accessToken;
            }

            return string.Empty;
        }

        /// <summary>
        /// Handle oauth callback
        /// </summary>
        /// <param name="uri"></param>
        private void AuthenticationBroker_OnSuccess(string uri)
        {
            Debug.WriteLine(uri);
            FacebookOauthResult result = ParseOauthResult(uri);

            if (result.IsSuccess)
            {
                FacebookModel.Instance.FacebookAccessToken = result.AccessToken;
                FacebookModel.Instance.Expires = result.Expires.ToString();

                SaveAccessToken(result.AccessToken);
            }

            if (loginCallback != null)
            {
                loginCallback.Invoke(result);
            }
        }

        /// <summary>
        /// Build permission oauth string
        /// </summary>
        /// <param name="permissions"></param>
        /// <returns></returns>
        private string ConstructPermission(List<string> permissions)
        {
            string scope = string.Empty;

            if (permissions != null)
            {
                foreach (var item in permissions)
                {
                    scope += item;
                    scope += ",";
                }
            }

            if (!string.IsNullOrEmpty(scope))
            {
                scope = scope.Remove(scope.Length - 1);
            }
            
            return scope;
        }

        /// <summary>
        /// Parse oauth result data to acquire access token
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        private FacebookOauthResult ParseOauthResult(string uri)
        {
            uri = uri.Replace(FacebookConstants.FACEBOOK_REDIRECT_URI + "#", "");
            string finalUri = PlatformUtility.UrlDecode(uri);
            string[] paramList = finalUri.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);

            FacebookOauthResult result = new FacebookOauthResult();

            foreach (string item in paramList)
            {
                if (item.StartsWith("access_token="))
                {
                    result.AccessToken = item.Replace("access_token=", string.Empty);
                }
                else if (item.StartsWith("expires_in="))
                {
                    string timeValue = item.Replace("expires_in=", string.Empty);

                    long time;
                    long.TryParse(timeValue, out time);

                    result.Expires = DateTime.Now.Add(TimeSpan.FromSeconds(time));
                }
                else if (item.StartsWith("error="))
                {
                    result.AccessToken = item.Replace("error=", string.Empty);
                }
                else if (item.StartsWith("error_code="))
                {
                    result.Code = item.Replace("error_code=", string.Empty);
                }
                else if (item.StartsWith("error_description="))
                {
                    result.ErrorDescription = item.Replace("error_description=", string.Empty);
                }
                else if (item.StartsWith("error_reason="))
                {
                    result.ErrorReason = item.Replace("error_reason=", string.Empty);
                }
            }

            if (string.IsNullOrEmpty(result.AccessToken))
            {
                result.IsSuccess = false;
            }
            else
            {
                result.IsSuccess = true;
            }

            return result;
        }

        /// <summary>
        /// Save access token to local setting
        /// </summary>
        /// <param name="accessToken"></param>
        private void SaveAccessToken(string accessToken)
        {
            string key = FacebookConstants.FACEBOOK_STORE_ACCESS_TOKEN + FacebookModel.Instance.AppId;
            storageService.Save(key, accessToken);
        }
        #endregion

        #region Facebook Actions: Like, Share, Invite,...
        #region Share
        /// <summary>
        /// Open dialog to share a post on facebook timeline
        /// </summary>
        /// <param name="param"></param>
        public void ShareOnFacebook(ModelShareFacebook param, Action<FacebookActionResult> callback = null)
        {
            if (string.IsNullOrEmpty(FacebookModel.Instance.AppId))
            {
                throw new FacebookOauthException("AppId must not be empty");
            }

            if (string.IsNullOrEmpty(param.Href))
            {
                return;
            }

            actionCallback = callback;

            string actionUrl = ConstructShareUrl(FacebookModel.Instance.AppId, param);
            ShowActionDialog(actionUrl);
        }

        /// <summary>
        /// Open dialog to post on facebook timeline
        /// </summary>
        /// <param name="param"></param>
        public void PostToFacebook(ModelPostFacebook param, Action<FacebookActionResult> callback = null)
        {
            if (string.IsNullOrEmpty(FacebookModel.Instance.AppId))
            {
                throw new FacebookOauthException("AppId must not be empty");
            }

            if (string.IsNullOrEmpty(param.Link))
            {
                return;
            }

            actionCallback = callback;

            string actionUrl = ConstructPostUrl(FacebookModel.Instance.AppId, param);
            ShowActionDialog(actionUrl);
        }

        /// <summary>
        /// Open dialog to send a post on facebook timeline
        /// </summary>
        /// <param name="param"></param>
        public void SendToFacebook(ModelSendFacebook param, Action<FacebookActionResult> callback = null)
        {
            if (string.IsNullOrEmpty(FacebookModel.Instance.AppId))
            {
                throw new FacebookOauthException("AppId must not be empty");
            }

            if (string.IsNullOrEmpty(param.Link))
            {
                return;
            }

            actionCallback = callback;

            string actionUrl = ConstructSendUrl(FacebookModel.Instance.AppId, param);
            ShowActionDialog(actionUrl);
        }

        /// <summary>
        /// Open dialog to send facebook app request
        /// </summary>
        /// <param name="param"></param>
        /// <param name="callback"></param>
        public void AppRequest(ModelAppRequestFacebook param, Action<FacebookActionResult> callback = null)
        {
            if (string.IsNullOrEmpty(FacebookModel.Instance.AppId))
            {
                throw new FacebookOauthException("AppId must not be empty");
            }

            if (string.IsNullOrEmpty(param.Message))
            {
                return;
            }

            actionCallback = callback;

            string actionUrl = ConstrutAppRequestUrl(FacebookModel.Instance.AppId, param);
            ShowActionDialog(actionUrl);
        }

        /// <summary>
        /// Show a facebook page on dialog
        /// </summary>
        public void ShowFacebookPage(string objectId, Action<FacebookActionResult> callback = null)
        {
            actionCallback = callback;
            string actionUrl = FacebookConstants.FACEBOOK_URL + objectId;

            ShowActionDialog(actionUrl);
        }

        /// <summary>
        /// Display sharing dialog with type
        /// </summary>
        private void ShowActionDialog(string actionUrl)
        {
            if (!string.IsNullOrEmpty(actionUrl))
            {
                WebAuthenticationBroker authenticationBroker = new WebAuthenticationBroker();
                authenticationBroker.OnSuccess += Sharing_OnSuccess;
                authenticationBroker.OnClosed += Sharing_OnClosed;

                Func<string, bool> validateMethod = (arg) =>
                {
                    return !string.IsNullOrEmpty(arg) && arg.StartsWith(FacebookConstants.FACEBOOK_REDIRECT_URI);
                };

                authenticationBroker.Authenticate(actionUrl, ResponseTypes.Url, validateMethod, false);
            }
        }
        
        /// <summary>
        /// Build final share url
        /// </summary>
        /// <param name="shareUrl"></param>
        /// <returns></returns>
        private string ConstructShareUrl(string appId, ModelShareFacebook model)
        {
            string finalUrl = FacebookConstants.FACEBOOK_SHARE_URL + "?";
            finalUrl += "appId=" + appId;
            finalUrl += "&display=popup";
            finalUrl += "&href=" + PlatformUtility.UrlEncode(model.Href);
            finalUrl += "&redirect_uri=" + PlatformUtility.UrlEncode(FacebookConstants.FACEBOOK_REDIRECT_URI);

            if (!string.IsNullOrEmpty(model.Hashtag))
            {
                finalUrl += "&hashtag=" + model.Hashtag;
            }

            if (!string.IsNullOrEmpty(model.Quote))
            {
                finalUrl += "&quote=" + model.Quote;
            }

            return finalUrl;
        }

        /// <summary>
        /// Build final send url
        /// </summary>
        /// <param name="shareUrl"></param>
        /// <returns></returns>
        private string ConstructSendUrl(string appId, ModelSendFacebook model)
        {
            string finalUrl = FacebookConstants.FACEBOOK_SEND_URL + "?";
            finalUrl += "appId=" + appId;
            finalUrl += "&display=page";
            finalUrl += "&link=" + PlatformUtility.UrlEncode(model.Link);
            finalUrl += "&redirect_uri=" + PlatformUtility.UrlEncode(FacebookConstants.FACEBOOK_REDIRECT_URI);

            if (!string.IsNullOrEmpty(model.To))
            {
                finalUrl += "&to=" + model.To;
            }

            return finalUrl;
        }

        /// <summary>
        /// Build final post url
        /// </summary>
        /// <param name="shareUrl"></param>
        /// <returns></returns>
        private string ConstructPostUrl(string appId, ModelPostFacebook model)
        {
            string finalUrl = FacebookConstants.FACEBOOK_POST_URL + "?";
            finalUrl += "appId=" + appId;
            finalUrl += "&display=popup";
            finalUrl += "&link=" + PlatformUtility.UrlEncode(model.Link);
            finalUrl += "&redirect_uri=" + PlatformUtility.UrlEncode(FacebookConstants.FACEBOOK_REDIRECT_URI);

            if (!string.IsNullOrEmpty(model.Caption))
            {
                finalUrl += "&caption=" + PlatformUtility.UrlEncode(model.Caption);
            }

            if (!string.IsNullOrEmpty(model.Description))
            {
                finalUrl += "&description=" + PlatformUtility.UrlEncode(model.Description);
            }

            if (!string.IsNullOrEmpty(model.From))
            {
                finalUrl += "&from=" + model.From;
            }

            if (!string.IsNullOrEmpty(model.To))
            {
                finalUrl += "&to=" + model.To;
            }

            if (!string.IsNullOrEmpty(model.Name))
            {
                finalUrl += "&name=" + PlatformUtility.UrlEncode(model.Name);
            }

            if (!string.IsNullOrEmpty(model.Picture))
            {
                finalUrl += "&picture=" + PlatformUtility.UrlEncode(model.Picture);
            }

            if (!string.IsNullOrEmpty(model.Source))
            {
                finalUrl += "&source=" + PlatformUtility.UrlEncode(model.Source);
            }

            return finalUrl;
        }

        /// <summary>
        /// Build final app request url
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private string ConstrutAppRequestUrl(string appId, ModelAppRequestFacebook model)
        {
            string finalUrl = FacebookConstants.FACEBOOK_APP_REQUEST_URL + "?";
            finalUrl += "appId=" + appId;
            finalUrl += "&display=popup";
            finalUrl += "&message=" + PlatformUtility.UrlEncode(model.Message);
            finalUrl += "&redirect_uri=" + PlatformUtility.UrlEncode(FacebookConstants.FACEBOOK_REDIRECT_URI);
            finalUrl += "&sdk=2";

            if (!string.IsNullOrEmpty(model.To))
            {
                finalUrl += "&to=" + model.To;
            }

            if (!string.IsNullOrEmpty(model.Title))
            {
                finalUrl += "&title=" + PlatformUtility.UrlEncode(model.Title);
            }

            return finalUrl;
        }

        /// <summary>
        /// Handle oauth callback
        /// </summary>
        /// <param name="uri"></param>
        private void Sharing_OnSuccess(string uri)
        {
            Debug.WriteLine(uri);
            FacebookActionResult result = ParseActionResult(uri);

            if (actionCallback != null)
            {
                actionCallback.Invoke(result);
            }
        }

        /// <summary>
        /// Handle dialog closed event
        /// </summary>
        private void Sharing_OnClosed()
        {
            FacebookActionResult result = new FacebookActionResult();
            result.IsSuccess = false;
            result.ErrorMessage = "User canceled";

            if (actionCallback != null)
            {
                actionCallback.Invoke(result);
            }
        }

        /// <summary>
        /// Parse facebook action result string to result model
        /// </summary>
        /// <param name="result"></param>
        private FacebookActionResult ParseActionResult(string result)
        {
            result = PlatformUtility.UrlDecode(result);

            result = result.Replace(Facebook.FacebookConstants.FACEBOOK_REDIRECT_URI + "?", "");
            result = result.Replace("#_=_", "");

            FacebookActionResult resultModel = new FacebookActionResult();

            if (!string.IsNullOrEmpty(result))
            {
                string[] paramList = result.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string item in paramList)
                {
                    if (item.StartsWith("post_id="))
                    {
                        resultModel.PostId = item.Replace("post_id=", string.Empty);
                    }
                    else if (item.StartsWith("error_code="))
                    {
                        resultModel.ErrorCode = item.Replace("error_code=", string.Empty);
                    }
                    else if (item.StartsWith("error_message="))
                    {
                        resultModel.ErrorMessage = item.Replace("error_message=", string.Empty);
                    }
                    else if (item.StartsWith("request="))
                    {
                        resultModel.RequestId = item.Replace("request=", string.Empty);
                    }
                }

                if (string.IsNullOrEmpty(resultModel.ErrorMessage) && string.IsNullOrEmpty(resultModel.ErrorCode))
                {
                    resultModel.IsSuccess = true;
                }
            }
            else
            {
                resultModel.IsSuccess = true;
            }

            return resultModel;
        }
        #endregion
        #endregion

        #region API
        /// <summary>
        /// Handle if http request failed
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <param name="tag"></param>
        private void FacebookAPI_OnFailed(string code, string message, string tag)
        {
            Debug.WriteLine(tag + ": " + code + "-" + message);
        }

        /// <summary>
        /// Get user's facebook info
        /// </summary>
        public async Task<FacebookUserInfo> GetUserInfo(string userId)
        {
            return await facebookAPI.GetFacebookInfo(userId, "name");
        }
        #endregion
        #endregion
    }
}

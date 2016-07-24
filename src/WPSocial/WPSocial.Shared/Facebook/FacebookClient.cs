using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WPSocial.Facebook.Models;
using WPSocial.Facebook.Models.Data;

namespace WPSocial.Facebook
{
    public class FacebookClient
    {
        #region Fields
        private FacebookController facebookController;
        #endregion

        #region Events
        public event Action<FacebookOauthResult> OnFacebookLoginCompleted;
        public event Action<FacebookActionResult> OnActionCompleted;
        #endregion

        #region Methods
        public FacebookClient()
        {
            facebookController = new FacebookController();
        }

        /// <summary>
        /// Init facebook sdk with appId
        /// Note: appId and appSecret can acquire from you app dashboard
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        public static void Init(string appId)
        {
            FacebookModel.Instance.AppId = appId;
        }

        /// <summary>
        /// Set app secret key to sdk
        /// App secret is used for APIs that need more authorization
        /// </summary>
        /// <param name="appSecret"></param>
        public static void SetAppSecret(string appSecret)
        {
            FacebookModel.Instance.AppSecret = appSecret;
        }

        /// <summary>
        /// Authenticate facebook user
        /// </summary>
        public void Login()
        {
            facebookController.Authenticate(null, OnFacebookLoginCompleted);
        }

        /// <summary>
        /// Authenticate facebook user with permissions
        /// </summary>
        public void Login(List<string> permissions)
        {
            facebookController.Authenticate(permissions, OnFacebookLoginCompleted);
        }

        /// <summary>
        /// Logout current user
        /// </summary>
        public void Logout()
        {
            facebookController.Logout();
        }

        /// <summary>
        /// Share on facebook
        /// </summary>
        public void Share(string url, string hashtag = "", string quote = "")
        {
            ModelShareFacebook model = new ModelShareFacebook()
            {
                Href = url,
                Hashtag = hashtag,
                Quote = quote
            };

            facebookController.ShareOnFacebook(model, OnActionCompleted);
        }

        /// <summary>
        /// Send a post to facebook timeline
        /// </summary>
        /// <param name="url"></param>
        private void Send(string url, string toId = "")
        {
            ModelSendFacebook model = new ModelSendFacebook()
            {
                Link = url,
                To = toId
            };

            facebookController.SendToFacebook(model, OnActionCompleted);
        }

        /// <summary>
        /// Post a feed to facebook timeline
        /// </summary>
        /// <param name="url"></param>
        public void Post(string url, string description, string caption, string name, string pictureUrl = "", string mediaSource = "", string fromId = "", string toId = "")
        {
            ModelPostFacebook model = new ModelPostFacebook()
            {
                Link = url,
                Description = description,
                Caption = caption,
                Name = name,
                From = fromId,
                To = toId,
                Picture = pictureUrl,
                Source = mediaSource
            };

            facebookController.PostToFacebook(model, OnActionCompleted);
        }

        /// <summary>
        /// Show app request dialog to send request to facebook friends
        /// </summary>
        /// <param name="message"></param>
        /// <param name="toId"></param>
        public void ShowAppRequest(string message, string toId = "", string title = "")
        {
            ModelAppRequestFacebook model = new ModelAppRequestFacebook()
            {
                Message = message,
                To = toId,
                Title = title
            };

            facebookController.AppRequest(model, OnActionCompleted);
        }

        /// <summary>
        /// Show a facebook page on dialog that user could like it
        /// </summary>
        /// <param name="pageId"></param>
        public void LikePage(string pageId)
        {
            facebookController.ShowFacebookPage(pageId, OnActionCompleted);
        }

        /// <summary>
        /// Get facebook info
        /// </summary>
        /// <param name="userId"></param>
        public Task<FacebookUserInfo> GetUserInfo(string userId)
        {
            return facebookController.GetUserInfo(userId);
        }
        #endregion
    }
}

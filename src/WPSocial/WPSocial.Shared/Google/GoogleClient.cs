using System;

namespace WPSocial.Google
{
    public class GoogleClient
    {
        #region Singleton
        private static GoogleClient instance;
        public static GoogleClient Instance
        {
            get { return instance ?? (instance = new GoogleClient()); }
        }
        #endregion

        #region Events
        public event Action<GoogleOauthResult> GoogleLoginCompleted;
        #endregion

        #region Fields
        private GoogleController googleController;
        #endregion

        #region Methods
        private GoogleClient()
        {
            googleController = new GoogleController();
        }

        /// <summary>
        /// Init google sdk with clientId and clientSecret
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        public void Init(string clientId, string clientSecret)
        {
            GoogleModel.Instance.ClientId = clientId;
            GoogleModel.Instance.ClientSecret = clientSecret;
        }

        /// <summary>
        /// Login google account with specified scope
        /// </summary>
        /// <param name="scope"></param>
        public void Login(string scope)
        {
            googleController.Authenticate(scope, GoogleLoginCompleted);
        }

        /// <summary>
        /// Logout current user
        /// </summary>
        public void Logout()
        {
            googleController.Logout();
        }
        #endregion
    }
}

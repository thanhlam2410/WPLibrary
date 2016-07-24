namespace WPSocial.Facebook
{
    internal class FacebookConstants
    {
        public const string FACEBOOK_AUTH_URL_FORMAT = @"https://m.facebook.com/dialog/oauth?client_id={0}&response_type=token&display=touch&redirect_uri={1}";
        public const string FACEBOOK_POST_URL = @"https://m.facebook.com/dialog/feed";
        public const string FACEBOOK_SHARE_URL = @"https://m.facebook.com/dialog/share";
        public const string FACEBOOK_SEND_URL = @"http://m.facebook.com/dialog/send";
        public const string FACEBOOK_APP_REQUEST_URL = @"https://m.facebook.com/dialog/apprequests";
        public const string FACEBOOK_REDIRECT_URI = @"https://m.facebook.com/connect/login_success.html";
        public const string FACEBOOK_URL = @"https://m.facebook.com/";

        public const string FACEBOOK_STORE_ACCESS_TOKEN = "FACEBOOK_STORE_ACCESS_TOKEN";
    }
}

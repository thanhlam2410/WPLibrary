using System;
using System.Collections.Generic;
using System.Text;

namespace WPSocial.Google
{
    internal class GoogleConstants
    {
        public const string GOOGLE_AUTH_URL = @"https://accounts.google.com/o/oauth2/auth";
        public const string GOOGLE_AUTH_REDIRECT_URI = @"urn:ietf:wg:oauth:2.0:oob:auto";
        public const string GOOGLE_AUTH_FORMAT = "{0}?response_type=code&client_id={1}&redirect_uri={2}&scope={3}";

        public const string GOOGLE_STORE_REFRESH_TOKEN = "GOOGLE_STORE_REFRESH_TOKEN";
    }
}

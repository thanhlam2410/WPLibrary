using System;
using System.Collections.Generic;
using System.Text;

namespace WPSocial.Google
{
    public class GoogleOauthResult
    {
        public bool IsSuccess { get; set; }

        public string AccessToken { get; set; }

        public long ExpiredIn { get; set; }
        
        public string TokenType { get; set; }
        
        public string RefreshToken { get; set; }

        public string Error { get; set; }
    }
}

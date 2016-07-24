using System;
using System.Collections.Generic;
using System.Text;

namespace WPSocial.Facebook
{
    public class FacebookOauthResult
    {
        public string AccessToken { get; set; }

        public string Code { get; set; }

        public string Error { get; set; }

        public string ErrorDescription { get; set; }

        public string ErrorReason { get; set; }

        public DateTime Expires { get; set; }

        public bool IsSuccess { get; set; }

        public string State { get; set; }
    }
}

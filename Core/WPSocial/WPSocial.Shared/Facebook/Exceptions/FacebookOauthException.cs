using System;
using System.Collections.Generic;
using System.Text;

namespace WPSocial.Facebook.Exceptions
{
    public class FacebookOauthException : Exception
    {
        public FacebookOauthException(string message) : base(message)
        {

        }
    }
}

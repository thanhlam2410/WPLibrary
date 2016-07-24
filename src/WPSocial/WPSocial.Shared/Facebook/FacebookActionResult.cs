using System;
using System.Collections.Generic;
using System.Text;

namespace WPSocial.Facebook
{
    public class FacebookActionResult
    {
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Use for post facebook action
        /// </summary>
        public string PostId { get; set; }

        public string ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        /// <summary>
        /// Use for app request action
        /// </summary>
        public string RequestId { get; set; }
    }
}

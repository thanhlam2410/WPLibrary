using System;
using System.Collections.Generic;
using System.Text;

namespace WPSocial.Facebook.Models
{
    internal class ModelSendFacebook
    {
        /// <summary>
        /// A user ID of a recipient. Once the dialog comes up, the sender can specify additional people as recipients.
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// Required parameter. The URL that is being sent in the message.
        /// </summary>
        public string Link { get; set; }
    }
}

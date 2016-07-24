using System;
using System.Collections.Generic;
using System.Text;

namespace WPSocial.Facebook.Models
{
    public class ModelAppRequestFacebook
    {
        /// <summary>
        /// Either a user id,username or invite token, or a comma-separated list of user ids, usernames or invite tokens.
        /// These may or may not be a friend of the sender.
        /// If this is specified by the app, the sender will not have a choice of recipients. If not, the sender will see a multi-friend selector
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// Required. A plain-text message to be sent as part of the request.
        /// This text will surface in the App Center view of the request, but not on the notification jewel
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The title for the Dialog.Maximum length is 50 characters.
        /// </summary>
        public string Title { get; set; }
    }
}

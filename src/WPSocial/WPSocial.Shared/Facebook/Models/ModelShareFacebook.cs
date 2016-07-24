using System;
using System.Collections.Generic;
using System.Text;

namespace WPSocial.Facebook.Models
{
    internal class ModelShareFacebook
    {
        /// <summary>
        /// The link attached to this post. Required when using method share. Include open graph meta tags in the page at this URL to customize the story that is shared.
        /// </summary>
        public string Href { get; set; }

        /// <summary>
        /// A hashtag specified by the developer to be added to the shared content. People will still have the opportunity to remove this hashtag in the dialog. The hashtag should include the hash symbol, e.g. #facebook.
        /// </summary>
        public string Hashtag { get; set; }

        /// <summary>
        /// A quote to be shared along with the link, either highlighted by the user or predefined by the developer, as in a pull quote on an article.
        /// </summary>
        public string Quote { get; set; }
    }
}

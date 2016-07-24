using System;
using System.Collections.Generic;
using System.Text;

namespace WPSocial.Facebook.Models
{
    internal class ModelPostFacebook
    {
        /// <summary>
        /// The ID of the person posting the message. If this is unspecified, it defaults to the current person. If specified, it must be the ID of the person or of a page that the person administers.
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// The ID of the profile that this story will be published to. If this is unspecified, it defaults to the value of from. The ID must be a friend who also uses your app.
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// The link attached to this post.With the Feed Dialog, people can also share plain text status updates with no content from your app; just leave the link parameter empty.
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// The URL of a picture attached to this post.The picture must be at least 200px by 200px.
        /// </summary>
        public string Picture { get; set; }

        /// <summary>
        /// The URL of a media file (either SWF or MP3) attached to this post. If SWF, you must also specify picture to provide a thumbnail for the video.
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// The name of the link attachment.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The caption of the link (appears beneath the link name). If not specified, this field is automatically populated with the URL of the link.
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        /// The description of the link (appears beneath the link caption). If not specified, this field is automatically populated by information scraped from the link, typically the title of the page.
        /// </summary>
        public string Description { get; set; }
    }
}

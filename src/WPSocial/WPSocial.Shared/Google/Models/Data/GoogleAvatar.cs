using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WPSocial.Google.Models.Data
{
    public class GoogleAvatar
    {
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }
    }
}

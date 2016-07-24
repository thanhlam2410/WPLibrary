using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WPSocial.Google.Models.Data
{
    public class GoogleProfile
    {
        [JsonProperty(PropertyName = "displayName")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "image")]
        public GoogleAvatar Avatar { get; set; }
    }
}

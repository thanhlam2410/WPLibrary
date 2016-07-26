using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace WPCore.Json
{
    public static class JsonExtensions
    {
        public static string ToJson(this Dictionary<string, string> obj)
        {
            if (obj == null)
            {
                return string.Empty;
            }

            JObject jObject = new JObject();

            foreach (var kvp in obj)
            {
                jObject.Add(kvp.Key, kvp.Value);
            }

            if (jObject.Count > 0)
            {
                return jObject.ToString().Replace("\r\n", "");
            }

            return string.Empty;
        }

        public static string ToJson(this Dictionary<string, object> obj)
        {
            if (obj == null)
            {
                return string.Empty;
            }

            JObject jObject = new JObject();

            foreach (var kvp in obj)
            {
                var token = JToken.FromObject(kvp.Value);
                jObject.Add(kvp.Key, token);
            }

            if (jObject.Count > 0)
            {
                return jObject.ToString().Replace("\r\n", "");
            }

            return string.Empty;
        }
    }
}

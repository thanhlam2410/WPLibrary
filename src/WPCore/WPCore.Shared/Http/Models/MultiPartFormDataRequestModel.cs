using System.Collections.Generic;
using WPCore.Http.Enums;

namespace WPCore.Http.Models
{
    public class MultiPartFormDataRequestModel
    {
        public HttpMethods Method { get; set; }

        public Dictionary<string, string> Header { get; set; }

        public List<HttpData> Data { get; set; }

        public string Domain { get; set; }

        public string Tag { get; set; }

        public double Timeout { get; set; }
    }
}

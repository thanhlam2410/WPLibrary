using WPCore.Http.Enums;

namespace WPCore.Http.Models
{
    public class HttpData
    {
        public HttpDataTypes Type { get; set; }

        public object Value { get; set; }

        public string Key { get; set; }
    }
}

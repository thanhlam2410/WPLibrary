using System.Net;

namespace WPCore.Platform
{
    public class PlatformUtility
    {
        public static string UrlEncode(string data)
        {
            return WebUtility.UrlEncode(data);
        }

        public static string UrlDecode(string data)
        {
            return WebUtility.UrlDecode(data);
        }
    }
}

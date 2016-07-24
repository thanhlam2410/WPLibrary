using System.Net;

namespace WPCore.Platform
{
    public class PlatformUtility
    {
        public static string UrlEncode(string data)
        {
            return HttpUtility.UrlEncode(data);
        }

        public static string UrlDecode(string data)
        {
            return HttpUtility.UrlDecode(data);
        }
    }
}

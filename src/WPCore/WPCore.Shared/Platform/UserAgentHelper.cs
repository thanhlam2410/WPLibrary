using System;
using System.Collections.Generic;
using System.Linq;

#if WINDOWS_PHONE
using Microsoft.Phone.Controls;
#else
using Windows.UI.Xaml.Controls;
#endif

namespace WPCore.Platform
{
    public class UserAgentHelper
    {
        #region Fields
        private const string HTML =
        @"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.01 Transitional//EN"">
 
        <html>
        <head>
        <script language=""javascript"" type=""text/javascript"">
            function notifyUA() {
               window.external.notify(navigator.userAgent);
            }
        </script>
        </head>
        <body onload=""notifyUA();""></body>
        </html>";
        #endregion

        #region Properties
        public string Manufacturer { get; set; }
        public string OS { get; set; }
        public string Model { get; set; }
        public string LastUserAgent { get; set; }
        #endregion

        #region Methods
        public void GetUserAgent(Action<string, string, string, string> callback)
        {
#if WINDOWS_PHONE
            var browser = new WebBrowser();
            browser.IsScriptEnabled = true;
#else
            var browser = new WebView();
#endif

            browser.ScriptNotify += (sender, args) =>
            {
                string userAgent = args.Value;
                ParseUserAgent(userAgent);
                callback(userAgent, Manufacturer, Model, OS);
            };

            browser.NavigateToString(HTML);
        }

        public void ParseUserAgent(string userAgent)
        {
            if (string.IsNullOrEmpty(userAgent))
            {
                return;
            }

            if (userAgent == LastUserAgent)
            {
                return;
            }

            LastUserAgent = userAgent;

            int startIndex = -1;
            int endIndex = -1;

            startIndex = userAgent.IndexOf("(");

            if (startIndex >= 0)
            {
                userAgent = userAgent.Substring(startIndex);
                endIndex = userAgent.IndexOf(")");

                if (endIndex > 0)
                {
                    userAgent = userAgent.Substring(1, endIndex - 1);
                    string[] agentParams = userAgent.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                    GetAgentInfo(agentParams.ToList());
                }
            }
        }

        private void GetAgentInfo(List<string> agentInfos)
        {
            int indexOfManufacturer = -1;

            foreach (var info in agentInfos)
            {
                if (info.ToLower().Contains("windows phone"))
                {
                    OS = info;
                }

                if (info.ToLower().Contains("microsoft") || info.ToLower().Contains("nokia") ||
                    info.ToLower().Contains("htc") || info.ToLower().Contains("huawei") || info.ToLower().Contains("samsung"))
                {
                    Manufacturer = info;
                    indexOfManufacturer = agentInfos.IndexOf(info);
                }
            }

            if (indexOfManufacturer != -1 && indexOfManufacturer < agentInfos.Count - 1)
            {
                Model = agentInfos[indexOfManufacturer + 1];
            }
        }
        #endregion
    }
}

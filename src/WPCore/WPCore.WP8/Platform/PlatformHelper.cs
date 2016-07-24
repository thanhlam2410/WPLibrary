using Microsoft.Phone.Info;
using Microsoft.Phone.Net.NetworkInformation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Windows;
using System.Xml.Linq;
using Windows.ApplicationModel.Store;
using Windows.Networking.Connectivity;

namespace WPCore.Platform
{
    public class PlatformHelper
    {
        /// <summary>
        /// Get unique device id
        /// </summary>
        /// <returns></returns>
        public string GetDeviceId()
        {
            byte[] deviceArrayId = (byte[])Microsoft.Phone.Info.DeviceExtendedProperties.GetValue("DeviceUniqueId");
            string uniqueDeviceId = Convert.ToBase64String(deviceArrayId);
            return uniqueDeviceId;
        }

        /// <summary>
        /// Get network telco Id
        /// </summary>
        /// <returns></returns>
        public string GetNetworkCarrier()
        {
            var connectionProfiles = NetworkInformation.GetConnectionProfiles();

            foreach (var profile in connectionProfiles)
            {
                foreach (var properties in profile.GetType().GetProperties())
                {
                    if (properties.Name == "WwanConnectionProfileDetails")
                    {
                        object subObj = properties.GetValue(profile);

                        if (subObj != null)
                        {
                            foreach (var subProp in properties.PropertyType.GetProperties())
                            {
                                if (subProp.Name == "HomeProviderId")
                                {
                                    return subProp.GetValue(subObj).ToString();
                                }
                            }
                        }
                    }
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Check current telco name
        /// </summary>
        /// <returns></returns>
        public string GetNetworkCarrierName()
        {
            var sb = new StringBuilder();
            string carrier = DeviceNetworkInformation.CellularMobileOperator;
            sb.Append(!String.IsNullOrEmpty(carrier) ? carrier : "empty");

            return sb.ToString();
        }

        /// <summary>
        /// Find device's IP addresses
        /// </summary>
        /// <returns></returns>
        public string FindIPAddress()
        {
            List<string> ipAddresses = new List<string>();
            var hostnames = NetworkInformation.GetHostNames();

            if (hostnames != null)
            {
                foreach (var host in hostnames)
                {
                    if (host.IPInformation != null &&
                        (host.IPInformation.NetworkAdapter.IanaInterfaceType == 71
                        || host.IPInformation.NetworkAdapter.IanaInterfaceType == 6
                        || host.IPInformation.NetworkAdapter.IanaInterfaceType == 243
                        || host.IPInformation.NetworkAdapter.IanaInterfaceType == 244))
                    {
                        string ipAddress = host.DisplayName;
                        ipAddresses.Add(ipAddress);
                    }
                }
            }

            if (ipAddresses.Count < 1)
            {
                return string.Empty;
            }
            else
            {
                //use the last ip address if list contains more than one
                return ipAddresses[ipAddresses.Count - 1];
            }
        }

        /// <summary>
        /// Return system advertising Id
        /// </summary>
        /// <returns></returns>
        public string GetAdvertisingId()
        {
            return string.Empty;
        }

        /// <summary>
        /// Get client version
        /// </summary>
        /// <returns></returns>
        public string GetVersion()
        {
            string version = XDocument.Load("WMAppManifest.xml").Root.Element("App").Attribute("Version").Value;
            return version;
        }

        /// <summary>
        /// Get device friendly name
        /// </summary>
        /// <returns></returns>
        public string GetDeviceName()
        {
            object deviceName;
            DeviceExtendedProperties.TryGetValue("DeviceName", out deviceName);

            if (deviceName != null)
            {
                return deviceName.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Get current timestamp in miliseconds
        /// </summary>
        public long GetCurrentTimestamp()
        {
            DateTime startTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan time = DateTime.UtcNow - startTime;
            return (long)time.TotalMilliseconds;
        }

        /// <summary>
        /// Get device capable memory
        /// </summary>
        /// <returns></returns>
        public string GetDeviceLimitMemory()
        {
            long memory = DeviceStatus.DeviceTotalMemory / (1024 * 1024);
            return memory.ToString();
        }

        /// <summary>
        /// Get device current language
        /// </summary>
        /// <returns></returns>
        public string GetDeviceLanguage()
        {
            CultureInfo ci = Thread.CurrentThread.CurrentCulture;
            return ci.Name;
        }

        /// <summary>
        /// Get device screen size
        /// </summary>
        /// <returns></returns>
        public string GetScreenSize()
        {
            double width = Application.Current.Host.Content.ActualWidth;
            double height = Application.Current.Host.Content.ActualHeight;

            if (width < height)
            {
                return Math.Round(width, 0) + "x" + Math.Round(height, 0);
            }
            else
            {
                return Math.Round(height, 0) + "x" + Math.Round(width, 0);
            }
        }

        /// <summary>
        /// Get network type (1: 3G, 2: Wifi, 3: Other)
        /// </summary>
        /// <returns></returns>
        public NetworkTypes GetNetworkType()
        {
            NetworkInterfaceList interfacesList = new NetworkInterfaceList();

            foreach (NetworkInterfaceInfo specificInterface in interfacesList)
            {
                if (specificInterface.InterfaceType == NetworkInterfaceType.Wireless80211 || specificInterface.InterfaceType == NetworkInterfaceType.Ethernet)
                {
                    return NetworkTypes.Wifi;
                }
                else
                {
                    return NetworkTypes.Mobile;
                }
            }

            return NetworkTypes.Unknown;
        }

        /// <summary>
        /// Get package name
        /// </summary>
        /// <returns></returns>
        public string GetPackageName()
        {
            try
            {
                return CurrentApp.AppId.ToString();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("GetPackageName: " + ex.Message);
            }

            return string.Empty;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using Windows.ApplicationModel;
using Windows.Networking.Connectivity;
using Windows.Security.Cryptography;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.Storage.Streams;
using Windows.System.Profile;
using Windows.System.UserProfile;
using Windows.UI.Xaml;

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
            HardwareToken myToken = HardwareIdentification.GetPackageSpecificToken(null);
            IBuffer hardwareId = myToken.Id;

            string uniqueDeviceId = CryptographicBuffer.EncodeToBase64String(hardwareId);
            return uniqueDeviceId;
        }

        /// <summary>
        /// Get network telco
        /// </summary>
        /// <returns></returns>
        public string GetNetworkCarrier()
        {
            var connectionProfiles = NetworkInformation.GetConnectionProfiles();

            foreach (var profile in connectionProfiles)
            {
                if (profile.WwanConnectionProfileDetails != null)
                {
                    return profile.WwanConnectionProfileDetails.HomeProviderId;
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
            string carrier = "empty";
            var connectionProfiles = NetworkInformation.GetConnectionProfiles();

            foreach (var profile in connectionProfiles)
            {
                if (profile.IsWwanConnectionProfile)
                {
                    foreach (var networkName in profile.GetNetworkNames())
                    {
                        carrier = networkName;
                        break;
                    }
                }
            }

            return carrier;
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
            return AdvertisingManager.AdvertisingId;
        }

        /// <summary>
        /// Get client version
        /// </summary>
        /// <returns></returns>
        public string GetVersion()
        {
            PackageVersion pv = Package.Current.Id.Version;
            Version version = new Version(Package.Current.Id.Version.Major,
                Package.Current.Id.Version.Minor,
                Package.Current.Id.Version.Revision,
                Package.Current.Id.Version.Build);

            return version.ToString();
        }

        /// <summary>
        /// Get device friendly name
        /// </summary>
        /// <returns></returns>
        public string GetDeviceName()
        {
            EasClientDeviceInformation deviceInformation = new EasClientDeviceInformation();
            return deviceInformation.FriendlyName;
        }

        /// <summary>
        /// Get current timestamp in miliseconds
        /// </summary>
        public double GetCurrentTimestamp()
        {
            DateTime startTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan time = DateTime.UtcNow - startTime;
            return (long)time.TotalMilliseconds;
        }

        /// <summary>
        /// Get device capable memory
        /// </summary>
        /// <returns></returns>
        public virtual string GetDeviceLimitMemory()
        {
            return string.Empty;
        }

        /// <summary>
        /// Get device current language
        /// </summary>
        /// <returns></returns>
        public string GetDeviceLanguage()
        {
            CultureInfo ci = new CultureInfo(GlobalizationPreferences.Languages[0]);
            return ci.Name;
        }

        /// <summary>
        /// Get device screen size
        /// </summary>
        /// <returns></returns>
        public string GetScreenSize()
        {
            double width = Window.Current.Bounds.Width;
            double height = Window.Current.Bounds.Height;

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
            List<string> ipAddresses = new List<string>();
            var hostnames = NetworkInformation.GetHostNames();

            if (hostnames != null)
            {
                foreach (var host in hostnames)
                {
                    if (host.IPInformation != null &&
                        (host.IPInformation.NetworkAdapter.IanaInterfaceType == 71
                        || host.IPInformation.NetworkAdapter.IanaInterfaceType == 6))
                    {
                        return NetworkTypes.Wifi;
                    }
                    else if (host.IPInformation != null &&
                        (host.IPInformation.NetworkAdapter.IanaInterfaceType == 243
                        || host.IPInformation.NetworkAdapter.IanaInterfaceType == 244))
                    {
                        return NetworkTypes.Mobile;
                    }
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
            return Package.Current.Id.Name;
        }
    }
}

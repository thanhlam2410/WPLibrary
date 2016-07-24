using System;
using System.Diagnostics;
using Windows.Networking.Connectivity;

namespace WPCore.Platform
{
    public class NetworkHelper
    {
        #region Singleton
        private static volatile NetworkHelper instance;
        /// <summary>
        /// Singleton instance
        /// </summary>
        public static NetworkHelper Instance
        {
            get { return instance ?? (instance = new NetworkHelper()); }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Is network available
        /// </summary>
        public bool IsInternetAvailable { get; set; }

        /// <summary>
        /// Current network physical Id
        /// </summary>
        public string MacAddressId { get; set; }
        #endregion

        #region Events
        public event Action OnConnectionAvailable;
        #endregion

        #region Methods
        /// <summary>
        /// Constructor
        /// </summary>
        private NetworkHelper()
        {
            Init();
            NetworkInformation.NetworkStatusChanged += NetworkInformation_NetworkStatusChanged;
        }

        /// <summary>
        /// Init network info
        /// </summary>
        private void Init()
        {
            try
            {
                ConnectionProfile internetConnectionProfile = NetworkInformation.GetInternetConnectionProfile();

                if (internetConnectionProfile != null)
                {
                    IsInternetAvailable = (internetConnectionProfile.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.None);

                    if (internetConnectionProfile.NetworkAdapter != null)
                    {
                        MacAddressId = internetConnectionProfile.NetworkAdapter.NetworkAdapterId.ToString();
                    }
                }
            }
            catch
            {
                IsInternetAvailable = true;
            }
        }

        /// <summary>
        /// Handle network status changed
        /// </summary>
        /// <param name="sender"></param>
        private void NetworkInformation_NetworkStatusChanged(object sender)
        {
            try
            {
                ConnectionProfile internetConnectionProfile = NetworkInformation.GetInternetConnectionProfile();

                if (internetConnectionProfile != null)
                {
                    NetworkConnectivityLevel connectivityLevel = internetConnectionProfile.GetNetworkConnectivityLevel();

                    switch (connectivityLevel)
                    {
                        case NetworkConnectivityLevel.None:
                            {
                                Debug.WriteLine("No internet access");
                                IsInternetAvailable = false;
                                break;
                            }
                        default:
                            {
                                Debug.WriteLine("Internet access");
                                IsInternetAvailable = true;

                                if (OnConnectionAvailable != null)
                                {
                                    OnConnectionAvailable.Invoke();
                                }

                                break;
                            }
                    }
                }
                else
                {
                    Debug.WriteLine("Cannot detect network");
                    IsInternetAvailable = false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Network Status Changed: " + ex.Message);
            }
        }
        #endregion
    }
}

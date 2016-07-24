using System;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace WPCore.Location
{
    public class LocationHelper
    {
        #region Constants
        public const string LONGITUDE = "Longitude";
        public const string LATITUDE = "Latitude";
        #endregion

        #region Fields
        private Geolocator locator;
        #endregion

        #region Methods
        /// <summary>
        /// Constructor
        /// </summary>
        public LocationHelper()
        {
            locator = new Geolocator();
            locator.MovementThreshold = 1;

            locator.StatusChanged += GeolocatorStatusChanged;
            locator.PositionChanged += DevicePositionChanged;
        }

        /// <summary>
        /// Active when device position has changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void DevicePositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {

        }

        /// <summary>
        /// Active when capability of position access has changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void GeolocatorStatusChanged(Geolocator sender, StatusChangedEventArgs args)
        {
            PositionStatus status = args.Status;

            switch (status)
            {
                case PositionStatus.Ready:
                    {
                        break;
                    }
                case PositionStatus.Disabled:
                    {
                        break;
                    }
                case PositionStatus.NotAvailable:
                    {
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        /// <summary>
        /// Acquire device location
        /// </summary>
        public async Task<LocationModel> GetDeviceLocation()
        {
            LocationModel location = new LocationModel();

            try
            {
                Geolocator locator = new Geolocator();
                Geoposition position = await locator.GetGeopositionAsync();

                if (position != null)
                {
                    location.IsSuccess = true;
                    location.ErrorMessage = string.Empty;
#if WINDOWS_PHONE
                    location.Longitude = position.Coordinate.Longitude;
                    location.Latitude = position.Coordinate.Latitude;
#else
                    location.Longitude = position.Coordinate.Point.Position.Longitude;
                    location.Latitude = position.Coordinate.Point.Position.Latitude;
#endif
                }
                else
                {
                    location.IsSuccess = false;
                    location.ErrorMessage = "Unknown Error - Get location failed";
                    location.Longitude = -1;
                    location.Latitude = -1;
                }

                return location;
            }
            catch (Exception ex)
            {
                location.IsSuccess = false;
                location.ErrorMessage = ex.Message;
                location.Longitude = -1;
                location.Latitude = -1;

                return location;
            }
        }
#endregion
    }
}

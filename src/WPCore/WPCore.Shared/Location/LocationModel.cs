namespace WPCore.Location
{
    public class LocationModel
    {
        /// <summary>
        /// If location data is available, it must be true
        /// Otherwise, an error has occured
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Error message if any
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Location longitude
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// Location latitude
        /// </summary>
        public double Latitude { get; set; }
    }
}

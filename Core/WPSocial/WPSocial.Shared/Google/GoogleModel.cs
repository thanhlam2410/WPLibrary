using System;
using System.Collections.Generic;
using System.Text;

namespace WPSocial.Google
{
    internal class GoogleModel
    {
        #region Singleton
        private static volatile GoogleModel instance;
        public static GoogleModel Instance
        {
            get { return instance ?? (instance = new GoogleModel()); }
        }
        #endregion

        #region Properties
        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }
        #endregion

        #region Methods
        private GoogleModel()
        {

        }
        #endregion
    }
}

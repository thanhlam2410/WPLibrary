using System;
using System.Collections.Generic;
using System.Text;

namespace WPSocial.Facebook
{
    internal class FacebookModel
    {
        #region Singleton
        private static volatile FacebookModel instance;
        public static FacebookModel Instance
        {
            get { return instance ?? (instance = new FacebookModel()); }
        }
        #endregion

        #region Properties
        public string AppId { get; set; }

        public string AppSecret { get; set; }

        public string FacebookAccessToken { get; set; }

        public string Expires { get; set; }
        #endregion
    }
}

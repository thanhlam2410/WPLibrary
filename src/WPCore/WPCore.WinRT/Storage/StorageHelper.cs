using Windows.Storage;

namespace WPCore.Storage
{
    public class StorageHelper
    {
        #region Fields
        private ApplicationDataContainer storageSettings;
        #endregion

        #region Methods
        public StorageHelper()
        {
            storageSettings = ApplicationData.Current.LocalSettings;
        }

        /// <summary>
        /// Save data to local with key
        /// </summary>
        public void Save(string key, object value)
        {
            if (!storageSettings.Values.ContainsKey(key))
            {
                storageSettings.Values.Add(key, value);
            }
            else
            {
                storageSettings.Values[key] = value;
            }
        }

        /// <summary>
        /// Load local data by key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object Load(string key)
        {
            if (storageSettings.Values.ContainsKey(key))
            {
                return storageSettings.Values[key];
            }

            return null;
        }

        /// <summary>
        /// Check whether key is exist in local
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsExist(string key)
        {
            return storageSettings.Values.ContainsKey(key);
        }

        /// <summary>
        /// Delete a key from settings
        /// </summary>
        /// <param name="key"></param>
        public void Delete(string key)
        {
            if (storageSettings.Values.ContainsKey(key))
            {
                storageSettings.Values.Remove(key);
            }
        }
        #endregion
    }
}

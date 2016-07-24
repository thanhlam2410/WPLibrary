using System.IO.IsolatedStorage;

namespace WPCore.Storage
{
    public class StorageHelper
    {
        #region Fields
        private IsolatedStorageSettings storageSettings;
        #endregion

        #region Methods
        public StorageHelper()
        {
            storageSettings = IsolatedStorageSettings.ApplicationSettings;
        }

        /// <summary>
        /// Save data to local with key
        /// </summary>
        public void Save(string key, object value)
        {
            if (!storageSettings.Contains(key))
            {
                storageSettings.Add(key, value);
            }
            else
            {
                storageSettings[key] = value;
            }

            storageSettings.Save();
        }

        /// <summary>
        /// Load local data by key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object Load(string key)
        {
            if (storageSettings.Contains(key))
            {
                return storageSettings[key];
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
            return storageSettings.Contains(key);
        }

        /// <summary>
        /// Delete a key from settings
        /// </summary>
        /// <param name="key"></param>
        public void Delete(string key)
        {
            if (storageSettings.Contains(key))
            {
                storageSettings.Remove(key);
            }
        }
        #endregion
    }
}

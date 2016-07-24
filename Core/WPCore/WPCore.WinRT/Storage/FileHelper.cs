using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace WPCore.Storage
{
    public class FileHelper
    {
        /// <summary>
        /// Read a file and return file content as string
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<string> ReadFileToString(string uri)
        {
            try
            {
                if (string.IsNullOrEmpty(uri))
                {
                    return string.Empty;
                }

                var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(uri));

                if (file == null)
                {
                    return string.Empty;
                }

                string content = string.Empty;

                using (Stream readStream = await file.OpenStreamForReadAsync())
                {
                    StreamReader reader = new StreamReader(readStream);
                    content = reader.ReadToEnd();
                }

                return content;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ReadFileToString: " + ex.Message);
                return string.Empty;
            }
        }

        /// <summary>
        /// Write a string to file content
        /// </summary>
        /// <param name="file"></param>
        /// <param name="content"></param>
        public async void WriteStringToFile(string uri, string content)
        {
            try
            {
                if (string.IsNullOrEmpty(uri))
                {
                    return;
                }

                var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(uri));

                if (file == null)
                {
                    return;
                }

                using (Stream writeStream = await file.OpenStreamForWriteAsync())
                {
                    StreamWriter writer = new StreamWriter(writeStream);
                    writer.Write(content);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("WriteStringToFile: " + ex.Message);
            }
        }
    }
}

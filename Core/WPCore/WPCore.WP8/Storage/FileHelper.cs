using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

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
            if (string.IsNullOrEmpty(uri))
            {
                return string.Empty;
            }

            try
            {
                var resource = Application.GetResourceStream(new Uri(uri, UriKind.RelativeOrAbsolute));

                if (resource == null)
                {
                    return string.Empty;
                }

                string data = string.Empty;

                using (StreamReader streamReader = new StreamReader(resource.Stream))
                {
                    data = await streamReader.ReadToEndAsync();
                    streamReader.DiscardBufferedData();
                    streamReader.Close();
                }

                return data;
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
        public void WriteStringToFile(string uri, string content)
        {
            try
            {
                if (string.IsNullOrEmpty(uri))
                {
                    return;
                }

                var resource = Application.GetResourceStream(new Uri(uri, UriKind.RelativeOrAbsolute));

                using (StreamWriter streamWriter = new StreamWriter(resource.Stream))
                {
                    streamWriter.Write(content);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("WriteStringToFile: " + ex.Message);
            }
        }
    }
}

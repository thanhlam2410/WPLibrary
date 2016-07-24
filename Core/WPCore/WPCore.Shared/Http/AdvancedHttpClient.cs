using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.IO;
using WPCore.Http.Models;
using WPCore.Http.Enums;

#if WINDOWS_PHONE
using System.Windows.Threading;
using System.Net.Http;
#else
using Windows.UI.Xaml;
using System.Net.Http;
#endif

namespace WPCore.Http
{
    public class AdvancedHttpClient
    {
        #region Methods
        /// <summary>
        /// Send a http request ---- Form Data
        /// Put an empty dictionary in data to send empty request body
        /// </summary>
        /// <param name="httpRequest"></param>
        public async Task<HttpResultModel> SendRequest(FormDataRequestModel httpRequest)
        {
            HttpResultModel result = new HttpResultModel();

            try
            {
                if (httpRequest != null && httpRequest.Data != null && !string.IsNullOrEmpty(httpRequest.Domain))
                {
                    System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();

                    //Add timeout
                    if (httpRequest.Timeout > 0)
                    {
                        client.Timeout = TimeSpan.FromSeconds(httpRequest.Timeout);
                    }
                    else
                    {
                        client.Timeout = TimeSpan.FromSeconds(30);
                    }

                    //Add header
                    client.DefaultRequestHeaders.Add("Cache-Control", "no-cache");

                    if (httpRequest.Header != null)
                    {
                        foreach (var item in httpRequest.Header)
                        {
                            client.DefaultRequestHeaders.Add(item.Key, item.Value);
                        }
                    }

                    //Print debug log
                    Debug.WriteLine(DateTime.Now + " " + httpRequest.Tag + ": " + httpRequest.Domain);
                    PrintData(httpRequest.Data, httpRequest.Tag);

                    HttpResponseMessage response = null;
                    
                    //Send request
                    if (httpRequest.Method == HttpMethods.POST)
                    {
                        FormUrlEncodedContent content = new FormUrlEncodedContent(httpRequest.Data);
                        response = await client.PostAsync(new Uri(httpRequest.Domain, UriKind.RelativeOrAbsolute), content);
                    }
                    else
                    {
                        response = await client.GetAsync(new Uri(httpRequest.Domain, UriKind.RelativeOrAbsolute));
                    }
                    
                    if (response != null && response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        Debug.WriteLine(DateTime.Now + " " + httpRequest.Tag + ": " + responseContent);

                        result.IsSuccess = true;
                        result.ResposeData = responseContent;
                    }
                    else
                    {
                        result.IsSuccess = false;

                        if (response != null)
                        {
                            result.ResponseCode = response.StatusCode.ToString();
                            result.ErrorMessage = response.ReasonPhrase;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// Send a http request ---- MultiPart Form Data
        /// Put an empty dictionary in data to send empty request body
        /// </summary>
        /// <param name="httpRequest"></param>
        public async Task<HttpResultModel> SendRequest(MultiPartFormDataRequestModel httpRequest)
        {
            HttpResultModel result = new HttpResultModel();

            try
            {
                if (httpRequest != null && httpRequest.Data != null && !string.IsNullOrEmpty(httpRequest.Domain))
                {
                    System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();

                    //Add timeout
                    if (httpRequest.Timeout > 0)
                    {
                        client.Timeout = TimeSpan.FromSeconds(httpRequest.Timeout);
                    }
                    else
                    {
                        client.Timeout = TimeSpan.FromSeconds(30);
                    }

                    //Add header
                    client.DefaultRequestHeaders.Add("Cache-Control", "no-cache");

                    if (httpRequest.Header != null)
                    {
                        foreach (var item in httpRequest.Header)
                        {
                            client.DefaultRequestHeaders.Add(item.Key, item.Value);
                        }
                    }

                    //Print debug log
                    Debug.WriteLine(DateTime.Now + " " + httpRequest.Tag + ": " + httpRequest.Domain);

                    //Send request
                    HttpResponseMessage response = null;

                    if (httpRequest.Method == HttpMethods.POST)
                    {
                        MultipartFormDataContent content = new MultipartFormDataContent();

                        foreach (var param in httpRequest.Data)
                        {
                            if (param.Type == HttpDataTypes.String)
                            {
                                content.Add(new StringContent(param.Value as string), param.Key);
                            }
                            else if (param.Type == HttpDataTypes.ByteArray)
                            {
                                if (param.Value is byte[])
                                {
                                    content.Add(new StreamContent(new MemoryStream(param.Value as byte[])), param.Key, string.Format("{0}.jpg", Guid.NewGuid()));
                                }
                            }
                        }

                        response = await client.PostAsync(new Uri(httpRequest.Domain, UriKind.RelativeOrAbsolute), content);
                    }
                    else
                    {
                        response = await client.GetAsync(new Uri(httpRequest.Domain, UriKind.RelativeOrAbsolute));
                    }

                    if (response != null && response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        Debug.WriteLine(DateTime.Now + " " + httpRequest.Tag + ": " + responseContent);

                        result.IsSuccess = true;
                        result.ResposeData = responseContent;
                    }
                    else
                    {
                        result.IsSuccess = false;

                        if (response != null)
                        {
                            result.ResponseCode = response.StatusCode.ToString();
                            result.ErrorMessage = response.ReasonPhrase;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// Send a http request ---- Raw Data
        /// Put an empty dictionary in data to send empty request body
        /// </summary>
        /// <param name="httpRequest"></param>
        public async Task<HttpResultModel> SendRequest(RawDataRequestModel httpRequest)
        {
            HttpResultModel result = new HttpResultModel();

            try
            {
                if (httpRequest != null && httpRequest.Data != null && !string.IsNullOrEmpty(httpRequest.Domain))
                {
                    //Add timeout
                    System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();

                    if (httpRequest.Timeout > 0)
                    {
                        client.Timeout = TimeSpan.FromSeconds(httpRequest.Timeout);
                    }
                    else
                    {
                        client.Timeout = TimeSpan.FromSeconds(30);
                    }

                    //Add header
                    client.DefaultRequestHeaders.Add("Cache-Control", "no-cache");

                    if (httpRequest.Header != null)
                    {
                        foreach (var item in httpRequest.Header)
                        {
                            client.DefaultRequestHeaders.Add(item.Key, item.Value);
                        }
                    }

                    //Print debug log
                    Debug.WriteLine(DateTime.Now + " " + httpRequest.Tag + ": " + httpRequest.Domain);
                    Debug.WriteLine(DateTime.Now + " " + httpRequest.Tag + ": " + httpRequest.Data);

                    //Send request
                    HttpResponseMessage response = null;

                    if (httpRequest.Method == HttpMethods.POST)
                    {
                        StringContent content = new StringContent(httpRequest.Data);
                        response = await client.PostAsync(new Uri(httpRequest.Domain, UriKind.RelativeOrAbsolute), content);
                    }
                    else
                    {
                        response = await client.GetAsync(new Uri(httpRequest.Domain, UriKind.RelativeOrAbsolute));
                    }

                    if (response != null && response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        Debug.WriteLine(DateTime.Now + " " + httpRequest.Tag + ": " + responseContent);

                        result.IsSuccess = true;
                        result.ResposeData = responseContent;
                    }
                    else
                    {
                        result.IsSuccess = false;

                        if (response != null)
                        {
                            result.ResponseCode = response.StatusCode.ToString();
                            result.ErrorMessage = response.ReasonPhrase;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// Send a http request ---- File Data
        /// Put an empty dictionary in data to send empty request body
        /// </summary>
        /// <param name="httpRequest"></param>
        public async Task<HttpResultModel> SendRequest(FileDataRequestModel httpRequest)
        {
            HttpResultModel result = new HttpResultModel();

            try
            {
                if (httpRequest != null && httpRequest.Data != null && !string.IsNullOrEmpty(httpRequest.Domain))
                {
                    System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();

                    //Add timeout
                    if (httpRequest.Timeout > 0)
                    {
                        client.Timeout = TimeSpan.FromSeconds(httpRequest.Timeout);
                    }
                    else
                    {
                        client.Timeout = TimeSpan.FromSeconds(30);
                    }

                    //Add header
                    client.DefaultRequestHeaders.Add("Cache-Control", "no-cache");

                    if (httpRequest.Header != null)
                    {
                        foreach (var item in httpRequest.Header)
                        {
                            client.DefaultRequestHeaders.Add(item.Key, item.Value);
                        }
                    }

                    //Print debug log
                    Debug.WriteLine(DateTime.Now + " " + httpRequest.Tag + ": " + httpRequest.Domain);

                    //Send request
                    HttpResponseMessage response = null;

                    if (httpRequest.Method == HttpMethods.POST)
                    {
                        ByteArrayContent content = new ByteArrayContent(httpRequest.Data);
                        response = await client.PostAsync(new Uri(httpRequest.Domain, UriKind.RelativeOrAbsolute), content);
                    }
                    else
                    {
                        response = await client.GetAsync(new Uri(httpRequest.Domain, UriKind.RelativeOrAbsolute));
                    }

                    if (response != null && response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        Debug.WriteLine(DateTime.Now + " " + httpRequest.Tag + ": " + responseContent);

                        result.IsSuccess = true;
                        result.ResposeData = responseContent;
                    }
                    else
                    {
                        result.IsSuccess = false;

                        if (response != null)
                        {
                            result.ResponseCode = response.StatusCode.ToString();
                            result.ErrorMessage = response.ReasonPhrase;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// Get byte array over Http protocol
        /// </summary>
        /// <param name="httpRequest"></param>
        /// <returns></returns>
        public async Task<byte[]> GetBytes(ResourceGetModel httpRequest)
        {
            try
            {
                if (httpRequest != null && !string.IsNullOrEmpty(httpRequest.Domain))
                {
                    //Add timeout
                    System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();

                    if (httpRequest.Timeout > 0)
                    {
                        client.Timeout = TimeSpan.FromSeconds(httpRequest.Timeout);
                    }
                    else
                    {
                        client.Timeout = TimeSpan.FromSeconds(30);
                    }

                    //Add header
                    client.DefaultRequestHeaders.Add("Cache-Control", "no-cache");

                    if (httpRequest.Header != null)
                    {
                        foreach (var item in httpRequest.Header)
                        {
                            client.DefaultRequestHeaders.Add(item.Key, item.Value);
                        }
                    }

                    //Print debug log
                    Debug.WriteLine(DateTime.Now + " " + httpRequest.Tag + ": " + httpRequest.Domain);

                    //Send request
                    byte[] response = await client.GetByteArrayAsync(new Uri(httpRequest.Domain, UriKind.RelativeOrAbsolute));
                    return response;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now + " " + httpRequest.Tag + ": " + ex.Message);
            }

            return null;
        }

        /// <summary>
        /// Get data stream over Http protocol
        /// </summary>
        /// <param name="httpRequest"></param>
        /// <returns></returns>
        public async Task<Stream> GetStream(ResourceGetModel httpRequest)
        {
            try
            {
                if (httpRequest != null && !string.IsNullOrEmpty(httpRequest.Domain))
                {
                    //Add timeout
                    System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();

                    if (httpRequest.Timeout > 0)
                    {
                        client.Timeout = TimeSpan.FromSeconds(httpRequest.Timeout);
                    }
                    else
                    {
                        client.Timeout = TimeSpan.FromSeconds(30);
                    }

                    //Add header
                    client.DefaultRequestHeaders.Add("Cache-Control", "no-cache");

                    if (httpRequest.Header != null)
                    {
                        foreach (var item in httpRequest.Header)
                        {
                            client.DefaultRequestHeaders.Add(item.Key, item.Value);
                        }
                    }

                    //Print debug log
                    Debug.WriteLine(DateTime.Now + " " + httpRequest.Tag + ": " + httpRequest.Domain);

                    //Send request
                    Stream response = await client.GetStreamAsync(new Uri(httpRequest.Domain, UriKind.RelativeOrAbsolute));
                    return response;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now + " " + httpRequest.Tag + ": " + ex.Message);
            }

            return null;
        }

        /// <summary>
        /// Print debug data
        /// </summary>
        /// <param name="data"></param>
        private void PrintData(Dictionary<string, string> data, string tag)
        {
            Debug.WriteLine(DateTime.Now + " " + tag + ": ");

            if (data == null || data.Count == 0)
            {
                Debug.WriteLine("empty");
            }
            
            foreach(var item in data)
            {
                Debug.WriteLine(item.Key + ": " + item.Value);
            }
        }
        #endregion
    }
}

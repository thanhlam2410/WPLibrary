using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using WPCore.Json.Enums;

namespace WPCore.Json
{
    public class JsonConverter
    {
        /// <summary>
        /// Parse JObject to a Model or JArray
        /// </summary>
        /// <typeparam name="T">Model or collection of Model</typeparam>
        /// <param name="data">Json object</param>
        /// <param name="jsonType">Type to serialize</param>
        /// <returns></returns>
        public static T GetData<T>(object data, JsonTypes jsonType)
        {
            try
            {
                switch (jsonType)
                {
                    case JsonTypes.Object:
                        {
                            if (data is JObject)
                            {
                                T dataObject = (data as JObject).ToObject<T>();
                                return dataObject;
                            }

                            return default(T);
                        }
                    case JsonTypes.Array:
                        {
                            if (data is JArray)
                            {
                                T dataObject = (data as JArray).ToObject<T>();
                                return dataObject;
                            }

                            return default(T);
                        }
                    default:
                        return default(T);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("JsonConverter: " + ex.Message);
                return default(T);
            }
        }

        /// <summary>
        /// Serialize object to json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetJson<T>(T obj)
        {
            try
            {
                return JsonConvert.SerializeObject(obj);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("JsonConverter: " + ex.Message);
                return string.Empty;
            }
        }

        /// <summary>
        /// Deserialize json to object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T GetObject<T>(string data)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(data);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("JsonConverter: " + ex.Message);
                return default(T);
            }
        }
    }
}

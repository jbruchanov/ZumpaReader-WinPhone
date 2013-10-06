using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using ZumpaReader.Model;

namespace ZumpaReader.WebService
{
    public abstract class WebService : IWebService
    {
        /// <summary>
        /// Parse any generic response from server
        /// </summary>
        /// <typeparam name="T">Type for expected result</typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static ContextResult<T> Parse<T>(string json)
        {
            return JsonConvert.DeserializeObject<ContextResult<T>>(json);
        }

        /// <summary>
        /// Generic class for any kind of response from WebService
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public class ContextResult<T>
        {
            [JsonProperty("Context")]
            public T Context { get; set; }

            [JsonProperty("HasError")]
            public bool HasError { get; set; }
        }

        /// <summary>
        /// Event called on successful download of items.
        /// Called in main thread.
        /// </summary>
        public event EventHandler<WSDownloadEventArgs> DownloadedItems;

        /// <summary>
        /// Called in any kind of error.
        /// Called in main thread.
        /// </summary>
        public event EventHandler<WSErrorEventArgs> Error;

        /// <summary>
        /// Download main page
        /// </summary>
        /// <param name="url">Specific url for older pages, pass null for latest one</param>
        public abstract void DownloadItems(string url = null);        

        /// <summary>
        /// Call event handlers about error
        /// </summary>
        /// <param name="err"></param>
        public void OnError(Exception err)
        {
            if (Error != null)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() => Error.Invoke(this, new WSErrorEventArgs { Error = err }));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        public void OnDownloadedItems(ContextResult<ZumpaItemsResult> result)
        {
            if (DownloadedItems != null)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() => DownloadedItems.Invoke(this, new WSDownloadEventArgs { Result = result.Context }));
            }
        }
    }
}

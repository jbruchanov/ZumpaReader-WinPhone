using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        /// Download main page
        /// </summary>
        /// <param name="url">Specific url for older pages, pass null for latest one</param>
        public abstract Task<WebService.ContextResult<ZumpaItemsResult>> DownloadItems(string url = null);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public abstract Task<WebService.ContextResult<string>> Login(string username, string password);        

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract Task<WebService.ContextResult<bool>> Logout();        
    }
}

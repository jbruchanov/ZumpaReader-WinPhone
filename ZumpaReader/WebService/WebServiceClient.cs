using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Threading;
using ZumpaReader.Model;

namespace ZumpaReader.WebService
{
    public class WebServiceClient : WebService
    {
        private string _baseUrl;

        private const string ITEMS = "/zumpa";

        private const string POST = "POST";
        private const string TYPE_JSON = "application/json";

        public WebServiceClient()
        {
            _baseUrl = ZumpaReaderResources.Instance[ZumpaReader.ZumpaReaderResources.Keys.WebServiceURL];
        }


        /// <summary>
        /// Download page of items, after loading OnDownloadedItems is called
        /// </summary>
        /// <param name="page">Option param for page url, if null main page</param>
        public override void DownloadItems(string url = null)
        {
            var webRequest = (HttpWebRequest)HttpWebRequest.CreateHttp(_baseUrl + ITEMS);
            webRequest.Method = POST;
            webRequest.ContentType = TYPE_JSON;            

            #region Download part         
            AsyncCallback downloader = new AsyncCallback((ac2) =>
            {
                try
                {
                    WebResponse wr = webRequest.EndGetResponse(ac2);
                    using (Stream os = wr.GetResponseStream())
                    {
                        StreamReader sr = new StreamReader(os);
                        string data = sr.ReadToEnd();

                        ContextResult<ZumpaItemsResult> response = Parse<ZumpaItemsResult>(data);      
                        OnDownloadedItems(response);
                    }
                }
                catch (Exception e)
                {
                    OnError(e);
                }
            });
            #endregion

            #region Upload part            
            AsyncCallback uploader = new AsyncCallback((ac1) =>
            {
                try
                {                                                        
                    using (Stream s = webRequest.EndGetRequestStream(ac1))
                    {
                        byte [] data = System.Text.Encoding.UTF8.GetBytes("{}");
                        s.Write(data, 0, data.Length);
                        s.Close();
                        webRequest.BeginGetResponse(downloader, null);
                    }
                }
                catch (Exception e)
                {
                    OnError(e);
                }
            });
            #endregion

            webRequest.BeginGetRequestStream(uploader, null);
        }
    }
}

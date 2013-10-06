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
using ZumpaReader_UnitTests.WebService;

namespace ZumpaReader.WebService
{
    public class WebServiceClient : ZumpaReader.WebService.IWebService
    {
        private string _baseUrl;

        private const string ITEMS = "/zumpa";

        public event EventHandler<WSDownloadEventArgs> OnDownloadedItems;

        public event EventHandler<WSErrorEventArgs> OnError;

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
        public void DownloadItems(string url = null)
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
                        //TODO: response.HasError handling                  
                        if (OnDownloadedItems != null)
                        {
                            OnDownloadedItems.Invoke(this, new WSDownloadEventArgs { Result = response.Context });
                        }
                    }
                }
                catch (Exception e)
                {
                    NotifyError(e);
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
                    NotifyError(e);
                }
            });
            #endregion

            webRequest.BeginGetRequestStream(uploader, null);
            //WebClient wc = new WebClient();
            //wc.OpenWriteCompleted += (object sender, OpenWriteCompletedEventArgs e) =>
            //{
            //    try
            //    {
            //        Stream s = e.Result;
            //        StreamReader streamReader = new StreamReader(s);
            //        string data = streamReader.ReadToEnd();
            //        s.Close();

            //        List<ZumpaItem> items = JsonConvert.DeserializeObject<List<ZumpaItem>>(data);
            //        if (OnDownloadedItems != null)
            //        {
            //            OnDownloadedItems.Invoke(this, new WSDownloadEventArgs { Data = items });
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        NotifyError(ex);
            //    }
            //};
            //wc.OpenWriteAsync(new Uri(ITEMS), "{}");
        }

        /// <summary>
        /// Call event handlers about error
        /// </summary>
        /// <param name="err"></param>
        private void NotifyError(Exception err)
        {
            if (OnError != null)
            {
                OnError.Invoke(this, new WSErrorEventArgs { Error = err });
            }
        }
        
        private void ParseResponse<T>(string json)
        {
            
        }

        private ContextResult<T> Parse<T>(string json)
        {
            return JsonConvert.DeserializeObject<ContextResult<T>>(json);
        }

        private class ContextResult<T>
        {
            [JsonProperty("Context")]
            public T Context {get;set;}

            [JsonProperty("HasError")]
            public bool HasError { get; set; }
        }
    }
}

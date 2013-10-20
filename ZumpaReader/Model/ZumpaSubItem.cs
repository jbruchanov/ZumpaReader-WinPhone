using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using ZumpaReader.Utils;

namespace ZumpaReader.Model
{
    public class ZumpaSubItem
    {
        [JsonProperty("ID")]
        public int ID { get; set; }

        [JsonProperty("ParentID")]
        public int ParentID { get; set; }

        [JsonProperty("AuthorReal")]
        public string AuthorReal { get; set; }

        [JsonProperty("AuthorFake")]
        public string AuthorFake { get; set; }

        [JsonProperty("Body")]
        public string Body { get; set; }

        [JsonProperty("Time")]
        public long Time { get; set; }

        [JsonProperty("HasRespondForYou")]
        public bool HasRespondForYou { get; set; }

        [JsonProperty("HasInsideUris")]
        public bool HasInsideUris { get; set; }

        [JsonProperty("InsideUris")]
        public List<String> InsideUris { get; set; }

        [JsonProperty("Survey")]
        public Survey Survey { get; set; }

        private string _author;
        public string Author
        {
            get
            {
                if(_author == null){
                    _author = string.IsNullOrEmpty(AuthorFake) ? AuthorReal : AuthorFake;
                }
                return _author;
            }
        }
        
        private string _date;
        public string ReadableDateTime
        {
            get
            {
                if (_date == null)
                {
                    _date = StringUtils.ConvertDateTime(Time);
                }
                return _date;
            }
        }
    }
}

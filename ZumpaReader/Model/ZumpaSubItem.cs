using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

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

        private static int TIME_OFFSET = 10000;
        private string _date;
        public string ReadableDateTime
        {
            get
            {
                if (_date == null)
                {
                    long t = (Time + (DateTimeOffset.Now.Offset.Hours * 3600000)) * TIME_OFFSET;
                    _date = (Time > 86400000)
                            ? new DateTime(t).AddYears(1969).AddDays(-1).ToString("d.MM.yyyy HH:mm.ss", CultureInfo.InvariantCulture)
                            : new DateTime(t).ToString("HH:mm");
                }
                return _date;
            }
        }
    }
}

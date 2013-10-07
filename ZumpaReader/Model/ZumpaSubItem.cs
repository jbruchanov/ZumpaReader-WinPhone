using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
    }
}

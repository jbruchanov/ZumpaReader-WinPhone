using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZumpaReader.Model
{
    public class ZumpaItemsResult
    {
        [JsonProperty("NextPage")]
        public string NextPage { get; set; }

        [JsonProperty("PreviousPage")]
        public string PreviousPage { get; set; }

        [JsonProperty("Items")]
        public List<ZumpaItem> Items { get; set; }
    }
}

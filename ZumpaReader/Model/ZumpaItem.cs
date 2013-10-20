using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using ZumpaReader.Utils;

namespace ZumpaReader.Model
{
    public class ZumpaItem
    {
        [JsonProperty("Subject")]
        public string Subject { get; set; }

        [JsonProperty("ID")]
        public int ID { get; set; }

        [JsonProperty("Author")]
        public string Author { get; set; }

        [JsonProperty("Time")]
        public long Time { get; set; }

        [JsonProperty("Responds")]
        public int Responses { get; set; }

        [JsonProperty("HasRespondForYou")]
        public bool HasResponseForYou { get; set; }

        [JsonProperty("IsFavourite")]
        public bool IsFavourite { get; set; }

        [JsonProperty("HasBeenRead")]
        public bool HasBeenRead { get; set; }

        [JsonProperty("IsNewOne")]
        public bool IsNewOne { get; set; }

        [JsonProperty("ItemsUrl")]
        public string ItemsUrl { get; set; }        

        [JsonProperty("LastAnswerAuthor")]
        public string LastAnswerAuthor { get; set; }

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

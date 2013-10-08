using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ZumpaReader.Model
{
    public class ZumpaItem
    {
        private static int TIME_OFFSET = 10000;
        
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

        private string _date;

        public string ReadableDateTime
        {
            get {
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

        [JsonProperty("LastAnswerAuthor")]
        public string LastAnswerAuthor { get; set; }
    }
}

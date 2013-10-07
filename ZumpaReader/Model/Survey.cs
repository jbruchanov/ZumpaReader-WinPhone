using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZumpaReader.Model
{
    public class Survey
    {
        [JsonProperty("Question")]
        public string Question { get; set; }

        [JsonProperty("Responds")]
        public int Responds { get; set; }

        [JsonProperty("ID")]
        public int ID { get; set; }

        [JsonProperty("Answers")]
        public List<string> Answers { get; set; }

        [JsonProperty("Percents")]
        public List<int> Percents { get; set; }

        [JsonProperty("VotedItem")]
        public int VotedItem { get; set; }

        public Survey()
        {
            VotedItem = -1;
        }
    }
}

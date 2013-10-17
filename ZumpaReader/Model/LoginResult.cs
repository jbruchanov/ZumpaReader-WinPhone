using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZumpaReader.Model
{
    public class LoginResult
    {
        [JsonProperty("UID")]
        public string UID { get; set; }

        [JsonProperty("Cookies")]
        public string Cookies { get; set; }

        [JsonProperty("ZumpaResult")]
        public string ZumpaResult { get; set; }
        
        [JsonProperty("Result")]
        public bool Result { get; set; }
    }
}

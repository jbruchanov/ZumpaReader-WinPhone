using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZumpaReader.Model
{
    public class ZumpaItem
    {
        [Newtonsoft.Json.JsonProperty("Subject")]
        public string Subject { get; set; }
    }
}

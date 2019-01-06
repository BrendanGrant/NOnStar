using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace NOnStar.Types
{
    public class UpgradeError
    {
        public string Error { get; set; }
        [JsonProperty("error_description")]
        public string Description { get; set; }
    }
}

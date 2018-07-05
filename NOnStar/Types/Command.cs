using System;
using System.Collections.Generic;
using System.Text;

namespace NOnStar.Types
{
    class Command
    {
        public string name { get; set; }
        public string description { get; set; }
        public string url { get; set; }
        public bool isPrivSessionRequired { get; set; }
    }
}

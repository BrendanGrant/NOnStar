using System;
using System.Collections.Generic;
using System.Text;

namespace NOnStar.CommandAndControl
{
    class CommandResponse
    {
        public DateTime requestTime { get; set; }
        public DateTime completionTime { get; set; }
        public string url { get; set; }
        public string status { get; set; }
        public string type { get; set; }
        public Body body { get; set; }
    }
}

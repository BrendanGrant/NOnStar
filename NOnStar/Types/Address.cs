using System;
using System.Collections.Generic;
using System.Text;

namespace NOnStar.Types
{
    class Address
    {
        public string addressLine1 { get; set; }
        public string city { get; set; }
        public string provinceOrStateCode { get; set; }
        public string countryCode { get; set; }
        public string postalCode { get; set; }
    }
}

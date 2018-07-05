using System;
using System.Collections.Generic;
using System.Text;

namespace NOnStar.Types
{
    class Dealer
    {
        public string type { get; set; }
        public string name { get; set; }
        public string businessAssociateCode { get; set; }
        public string active { get; set; }
        public string phoneNo { get; set; }
        public string faxNo { get; set; }
        public string websiteURL { get; set; }
        public Address address { get; set; }
        public string dealerID { get; set; }
        public string dealerCode { get; set; }
        public string divisionCode { get; set; }
    }
}

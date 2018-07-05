using System;
using System.Collections.Generic;
using System.Text;

namespace NOnStar.Types
{
    class Vehicle
    {
        public string vin { get; set; }
        public string make { get; set; }
        public string model { get; set; }
        public string year { get; set; }
        public string manufacturer { get; set; }
        public string phone { get; set; }
        public string unitType { get; set; }
        public string onstarStatus { get; set; }
        public string url { get; set; }
        public string isInPreActivation { get; set; }
        public LicensePlate licensePlate { get; set; }
        public InsuranceInfo insuranceInfo { get; set; }
        public string enrolledInContinuousCoverage { get; set; }
        public Commands commands { get; set; }
        public Features features { get; set; }
        public Dealers dealers { get; set; }
        public string propulsionType { get; set; }
    }
}

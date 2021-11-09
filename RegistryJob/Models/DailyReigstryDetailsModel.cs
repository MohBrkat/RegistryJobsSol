using System;
using System.Collections.Generic;
using System.Text;

namespace RegistryJob.Models
{
    public class DailyReigstryDetailsModel
    {
        public int ListNumher { get; internal set; }
        public string RegistrantFirstName { get; internal set; }
        public string RegistrantLastName { get; internal set; }
        public string CoRegistrantFirstName { get; internal set; }
        public string CoRegistrantLastName { get; internal set; }
        public string RegistrantPhone { get; internal set; }
        public string RegistrantEmail { get; internal set; }
        public string CoRegistrantEmail { get; internal set; }
        public string CoRegistrantPhone { get; internal set; }
        public string Country { get; internal set; }
        public string City { get; internal set; }
        public string AddressLine1 { get; internal set; }
        public string AddressLine2 { get; internal set; }
        public string Zip { get; internal set; }
        public string Province { get; internal set; }
        public string County { get; internal set; }
    }
}

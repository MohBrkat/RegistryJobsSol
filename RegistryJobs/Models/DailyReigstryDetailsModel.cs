using System;
using System.Collections.Generic;
using System.Text;

namespace RegistryJob.Models
{
    public class DailyReigstryDetailsModel
    {
        public int ListNumber { get; set; }
        public string RegistrantFirstName { get; set; }
        public string RegistrantLastName { get; set; }
        public string CoRegistrantFirstName { get; set; }
        public string CoRegistrantLastName { get; set; }
        public string RegistrantPhone { get; set; }
        public string RegistrantEmail { get; set; }
        public string CoRegistrantEmail { get; set; }
        public string CoRegistrantPhone { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Zip { get; set; }
        public string Province { get; set; }
        public string County { get; set; }
        public DateTime ListCreatedDate { get; set; }
    }
}

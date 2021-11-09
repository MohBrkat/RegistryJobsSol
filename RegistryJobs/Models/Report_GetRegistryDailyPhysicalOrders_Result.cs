using System;
using System.Collections.Generic;
using System.Text;

namespace RegistryJob.Models
{
    using System;

    public partial class Report_GetRegistryDailyPhysicalOrders_Result
    {
        public int OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public string RecipientName { get; set; }
        public string Company { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string StreetLine1 { get; set; }
        public string StreetNumber { get; set; }
        public string StreetLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string ItemTitle { get; set; }
        public string SKU { get; set; }
        public Nullable<int> Quantity { get; set; }
        public string ItemWeight { get; set; }
        public decimal ItemPrice { get; set; }
        public string ItemCurrency { get; set; }
        public string ShippingMethod { get; set; }
    }
}

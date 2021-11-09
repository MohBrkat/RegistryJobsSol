using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RegistryJob.Models
{
    public class FundraisingOrdersModel
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
        public int? Quantity { get; set; }
        public string ItemWeight { get; set; }
        public decimal ItemPrice { get; set; }
        public string ItemCurrency { get; set; }
        public string ShippingMethod { get; set; }
    }
}

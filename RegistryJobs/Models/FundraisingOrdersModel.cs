using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RegistryJob.Models
{
    public partial class FundraisingOrdersModel
    {
        public int RowNo { get; set; }
        public string OrderNumber { get; set; }
        public string EventName { get; set; }
        public string OrderDate { get; set; }
        public string BuyerName { get; set; }
        public string BuyerEmail { get; set; }
        public string RecipientName { get; set; }
        public string RecipientEmail { get; set; }
        public string Company { get; set; }
        public string Phone { get; set; }
        public string StreetLine1 { get; set; }
        public string StreetNumber { get; set; }
        public string StreetLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string ProductTitle { get; set; }
        public string SKU { get; set; }
        public string Quantity { get; set; }
        public string ProductPrice { get; set; }
        public string Currency { get; set; }
        public string SubTotal { get; set; }
        public string Tax { get; set; }
        public string Shipping { get; set; }
        public string Total { get; set; }
        public string ShippingMethod { get; set; }
    }
}

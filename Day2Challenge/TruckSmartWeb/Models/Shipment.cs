using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TruckSmartWeb.Models
{
    public class Shipment
    {
        public Guid ShipmentID { get; set; }
        public Customer From { get; set; }
        public Customer To { get; set; }
        public Contractor Driver { get; set; }
        public DateTime Scheduled { get; set; }
        public DateTime? Shipped { get; set; }
        public bool Completed { get; set; }
    }
}
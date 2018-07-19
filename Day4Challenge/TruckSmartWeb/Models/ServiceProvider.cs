using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TruckSmartWeb.Models
{
    public class ServiceProvider
    {
        [Key]
        public Guid ProviderID { get; set; }
        public string Name { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        [DataType(DataType.PostalCode)]
        public string PostalCode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

    }
}
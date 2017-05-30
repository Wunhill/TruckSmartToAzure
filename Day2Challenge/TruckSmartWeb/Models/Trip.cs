using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TruckSmartWeb.Models
{
    public class Trip
    {
        public Guid TripID { get; set; }
        public Shipment Shipment { get; set; }
        public Driver Contractor { get; set; }
        public DateTime Date { get; set; }
        public GeoPoint Start { get; set; }
        public GeoPoint End { get; set; }
        public bool Delivered { get; set; }
        public string ClientIP { get; set; }
        public string ServerIP { get; set; }

    }
    public class GeoPoint
    {
        public GeoPoint()
        {

        }
        public GeoPoint(double Latitude, double Longitude)
        {
            this.Latitude = Latitude;
            this.Longitude = Longitude;
        }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
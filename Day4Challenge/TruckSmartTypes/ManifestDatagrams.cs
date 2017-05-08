using System;


namespace TruckSmartManifestTypes
{
    public class ManifestRequest
    {
        public string DriverID { get; set; }
        public string ShipmentID { get; set; }
        public DateTime Date { get; set; }
        public Point Start { get; set; }
        public Point End { get; set; }
        public bool Delivered { get; set; }
        public string ServerAddress { get; set; }
        public string ClientAddress { get; set; }
    }
    public class Point
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
    public class ManifestResponse
    {
        public double Distance { get; set; }
        public string ClientAddress { get; set; }
        public string ServerAddress { get; set; }
        public ManifestStatus Status { get; set; }
    }

    public enum ManifestStatus
    {
        Accepted,
        Pending,
        Rejected
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistanceEngine
{
    public class RouteDistance
    {
        public double GetDistance(GeoPoint from, GeoPoint to)
        {
            var earthRadiusKm = 6371;

            var dLat = degreesToRadians(to.Latitude - from.Latitude);
            var dLon = degreesToRadians(to.Longitude - from.Longitude);

            var lat1 = degreesToRadians(to.Latitude);
            var lat2 = degreesToRadians(from.Latitude);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2) * Math.Cos(lat1) * Math.Cos(lat2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return earthRadiusKm * c;
        }

        /// <summary>
        /// Converts a value from degrees to radians.  This is a support function for ProcessMessage
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private static double degreesToRadians(double p)
        {
            return p * Math.PI / 180.0;

        }
    }
    public class GeoPoint
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}

using System;
using System.IO;
using Newtonsoft.Json;
using TruckSmartManifestTypes;

namespace TruckSmartWeb.Models
{
    /// <summary>
    /// Handles all data processing for shipment manifests.
    /// </summary>
    public class ManifestModel
    {
        /// <summary>
        /// Accept and process a Manifest request.  The algorythm is based on a StackOverflow post:
        /// http://stackoverflow.com/questions/365826/calculate-distance-between-2-gps-coordinates
        /// I did some basic testing and it seems pretty legitimate.
        /// </summary>
        /// <param name="datagram">Input request</param>
        /// <returns>Manifest Response - simple POCO with results</returns>
        public static ManifestResponse ProcessMessage(ManifestRequest datagram)
        {

            var earthRadiusKm = 6371;

            var dLat = degreesToRadians(datagram.End.Latitude - datagram.Start.Latitude);
            var dLon = degreesToRadians(datagram.End.Longitude - datagram.Start.Longitude);

            var lat1 = degreesToRadians(datagram.End.Latitude);
            var lat2 = degreesToRadians(datagram.Start.Latitude);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2) * Math.Cos(lat1) * Math.Cos(lat2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var distance = earthRadiusKm * c;
            var output =  new ManifestResponse() { ServerAddress = System.Environment.MachineName, ClientAddress = datagram.ClientAddress, Distance = distance, Status = distance < 1000 ? ManifestStatus.Accepted : ManifestStatus.Pending };
            saveData(datagram, output);
            return output;
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
        /// <summary>
        /// Save the data in the input datagram and the output datagram.  Currently this saves them both to disk.
        /// </summary>
        /// <param name="datagram">Input POCO</param>
        /// <param name="output">Result POCO</param>
        private static void saveData(ManifestRequest datagram, ManifestResponse output)
        {
            string fileNameBase = string.Format("{0}@{1}", datagram.DriverID, datagram.ShipmentID);
            var drives = DriveInfo.GetDrives();
            //DriveInfo drive = null;
            ////Get the last drive on the machine
            //for (int lcv = 0; lcv < drives.Length; lcv++)
            //{
            //    if (drives[lcv].IsReady && drives[lcv].DriveType == DriveType.Fixed)
            //    {
            //        drive = drives[lcv];
            //    }
            //}
            //string path = drive.RootDirectory.FullName + "data";
            string path = System.Configuration.ConfigurationManager.AppSettings["dataFolder"];
            createDirectory(path);
            createDirectory(path + @"\datagrams");
            createDirectory(path + @"\results");

            string inputFile = string.Format("{0}\\datagrams\\{1}.datagram.json", path, fileNameBase);
            string outputFile = string.Format("{0}\\results\\{1}.result.json", path, fileNameBase);
            File.WriteAllText(inputFile, JsonConvert.SerializeObject(datagram));
            File.WriteAllText(outputFile, JsonConvert.SerializeObject(output));

        }

        /// <summary>
        /// Creates a directory if it doesn't exist.  This is a support function for saveData
        /// </summary>
        /// <param name="path">The path of the directory to create</param>
        private static void createDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }


    }
}

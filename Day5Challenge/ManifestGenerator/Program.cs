using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TruckSmartManifestTypes;

namespace ManifestGenerator
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string serverFQDN = string.Empty;
            string trip = string.Empty;
            string[] trips = { "GK", "NYtoCA", "CHItoDAL" };
            bool interactive = false;

            #region command line parameter parsing
            //If command line parameters are included, the first is the FQDN and the second is the test datagram
            if (args.Length > 0)
            {
                serverFQDN = args[0];
                if (args.Length > 1)
                {
                    int tripArg = -1;
                    if (int.TryParse(args[1], out tripArg))
                    {
                        if (tripArg >= 0 && tripArg < trips.Length)
                        {
                            trip = trips[tripArg];
                        }
                    }
                    else
                    {
                        trip = args[1];
                    }
                }
            }
            #endregion

            #region User input for serverFQDN
            while (serverFQDN == string.Empty)
            {
                Console.Write("Enter the FQDN of the manifest server and press enter:");
                serverFQDN = Console.ReadLine();
            }
            #endregion

            #region User inut for Datagram (by number)
            do
            {
                while (trip == string.Empty)
                {
                    Console.WriteLine("Enter the number for a test manifest and press enter:");
                    Console.WriteLine("\t0 - GK Delivery (Cary, NC to Atlanta, GA)");
                    Console.WriteLine("\t1 - Coast to Coast (New York, NY to San Jose, CA)");
                    Console.WriteLine("\t2 - North to South (Chicago, IL to Dallas, TX)");
                    string tripInput = Console.ReadLine();
                    int tripArg = -1;
                    int.TryParse(tripInput, out tripArg);
                    if (tripArg >= 0 && tripArg < trips.Length)
                    {
                        trip = trips[tripArg];
                    }
                    interactive = true;

                }
                #endregion

                string baseFolder = System.Configuration.ConfigurationManager.AppSettings["TestDatagramFolder"];
                string outputFileName = string.Format("{0}Result-{1:yyyyMMdd-T-HHmmss}.json", baseFolder, DateTime.Now);
                string inputFileName = string.Format("{0}{1}Datagram.json", baseFolder, trip);

                var datagram = JsonConvert.DeserializeObject<ManifestRequest>(File.ReadAllText(inputFileName));
                datagram.ServerAddress = serverFQDN;

                var client = new HttpClient();
                client.BaseAddress = new Uri(serverFQDN);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                Task.Run(async () =>
                {
                    try
                    {
                        var response = await client.PostAsJsonAsync("api/manifest", datagram);
                        var result = await response.Content.ReadAsAsync<ManifestResponse>();
                        var output = JsonConvert.SerializeObject(result, Formatting.Indented);
                        Console.WriteLine("Result:\n{0}", output);
                        File.WriteAllText(outputFileName, output);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("There was an error processing the manifest: \n{0}", ex.ToString());
                    }
                }).Wait();
                if(interactive)
                {
                    Console.WriteLine("");
                    Console.WriteLine("Enter the number for a test manifest and press enter:");
                    Console.WriteLine("\t0 - GK Delivery (Cary, NC to Atlanta, GA)");
                    Console.WriteLine("\t1 - Coast to Coast (New York, NY to San Jose, CA)");
                    Console.WriteLine("\t2 - North to South (Chicago, IL to Dallas, TX)");
                    Console.WriteLine("\tEnter anything else to quit.");
                    string tripInput = Console.ReadLine();
                    int tripArg = -1;
                    int.TryParse(tripInput, out tripArg);
                    if (tripArg >= 0 && tripArg < trips.Length)
                    {
                        trip = trips[tripArg];
                    } else
                    {
                        interactive = false;
                    }

                }

            } while (interactive);
               Console.WriteLine("Done. Click Enter to close");
                Console.ReadLine();
        }
    }
}

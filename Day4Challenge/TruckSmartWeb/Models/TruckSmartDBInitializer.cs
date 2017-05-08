using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TruckSmartWeb.Models
{
    public class TruckSmartDBInitializer : DropCreateDatabaseIfModelChanges<TruckSmartContext>
    {
        protected override void Seed(TruckSmartContext context)
        {
            var testDriverID = ConfigurationManager.AppSettings["testDriverID"];
            var testCustomerID = new Guid(ConfigurationManager.AppSettings["testCustomerID"]);
            var testShipmentID = new Guid(ConfigurationManager.AppSettings["testShipmentID"]);

            //Create Test driver
            var testDriver = new Driver
            {
                DriverID = testDriverID,
                Email = "gkane@wunhill.com",
                Name = "George Kane"
            };
            context.Drivers.Add(testDriver);
            //Create additional driver
            context.Drivers.Add(new Driver
            {
                DriverID = Guid.NewGuid().ToString(),
                Name = "Glenda King",
                Email = "gking@wunhill.com"
            });


            //Create Test customer
            var testCustomer = new Customer
            {
                CustomerID = testCustomerID,
                Address = " 11000 Regency Pkwy, Cary, NC 27518",
                Name = "Great Kare Cleaning Supplies",
                Latitude = 33.8852989,
                Longitude = -84.4616176

            };
            context.Customers.Add(testCustomer);

            //Create additional Customers
            string[] customerNames = { "Georgia Kennels", "Growing Kids Clothing", "Glen Kirkpatrick Associates", "Glowing Kharma Yoga Studio", "Golden Kinetics" };
            string[] customerAddresses = { "600 Galleria Pkwy SE Ste 700, Atlanta, GA 30339", "1500 McConnor Parkway Suite 500, Schaumburg, IL 60173", "5010 Riverside Drive Riverside Commons, Building 4 Suite 100, Irving, TX 75039", "5201 Great America Parkway Suite 219, Santa Clara, CA 95054", "1 State Street Plaza 23rd Floor, New York, NY 10004" };
            GeoPoint[] customerLocation = { new GeoPoint(33.885464, -84.461330), new GeoPoint(42.057829, -88.042597), new GeoPoint(32.859284, -96.930241), new GeoPoint(37.406497, -121.976532), new GeoPoint(40.702986, -74.013323) };
            List<Customer> customers = new List<Customer>();
            for (int lcv = 0; lcv < 5; lcv++)
            {
                var c = new Customer
                {
                    CustomerID = Guid.NewGuid(),
                    Name = customerNames[lcv],
                    Address = customerAddresses[lcv],
                    Latitude = customerLocation[lcv].Latitude,
                    Longitude = customerLocation[lcv].Longitude
                };
                customers.Add(c);
                
            }
            context.Customers.AddRange(customers);
            //Create test Shipment
            var testShipment = new Shipment
            {
                ShipmentID = testShipmentID,
                From = testCustomer,
                To = customers[0],
                Driver = testDriver,
                Completed = false,
                Scheduled = DateTime.Now.AddDays(7),
                Shipped = null
            };
            context.Shipments.Add(testShipment);
            //Create additional shipments
            context.Shipments.Add(new Shipment
            {
                ShipmentID = Guid.NewGuid(),
                From = customers[1],
                To = customers[2],
                Driver = testDriver,
                Completed = true,
                Scheduled = DateTime.Now.AddDays(-7),
                Shipped = DateTime.Now.AddDays(-7)
            });
            context.Shipments.Add(new Shipment
            {
                ShipmentID = Guid.NewGuid(),
                From = customers[3],
                To = customers[4],
                Driver = null,
                Completed = false,
                Scheduled = DateTime.Now.AddDays(14),
                Shipped = null
            });
            context.Shipments.Add(new Shipment
            {
                ShipmentID = Guid.NewGuid(),
                From = customers[2],
                To = customers[4],
                Driver = null,
                Completed = false,
                Scheduled = DateTime.Now.AddDays(14),
                Shipped = null
            });
            context.Shipments.Add(new Shipment
            {
                ShipmentID = Guid.NewGuid(),
                From = customers[3],
                To = customers[2],
                Driver = null,
                Completed = false,
                Scheduled = DateTime.Now.AddDays(14),
                Shipped = null
            });
            var rnd = new Random();
            for(int lcv = 0; lcv < 100; lcv++)
            {
                context.ServiceProviders.Add(new ServiceProvider
                {
                    ProviderID = Guid.NewGuid(),
                    Latitude = 30f + 17f * rnd.NextDouble(),
                    Longitude = -76f - 48f*rnd.NextDouble(),
                    Name = string.Format("Emergency Provider {0}",lcv),
                    Phone = string.Format("(000) 555 {0:0000}", 10000f*rnd.NextDouble()),
                    PostalCode = string.Format("{0:00000}", 100000f * rnd.NextDouble())
                 
                });
            }

            //Create a bunch of random service providers
            context.SaveChanges();

        }
    }
}
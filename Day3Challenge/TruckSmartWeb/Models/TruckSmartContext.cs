using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Configuration;
using StackExchange.Redis;
using Newtonsoft.Json;

namespace TruckSmartWeb.Models
{
    public class TruckSmartContext:DbContext
    {
        private string driverID
        {
            get
            {
                return System.Web.HttpContext.Current.Session["DriverID"].ToString();
            }
        }
        #region redis setup
        //private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        //{
        //    string cacheConnection = ConfigurationManager.AppSettings["redis"];
        //    return ConnectionMultiplexer.Connect(cacheConnection);
        //});

        //public static ConnectionMultiplexer Connection
        //{
        //    get
        //    {
        //        return lazyConnection.Value;
        //    }
        //}

        #endregion

        #region Database initialization
        static TruckSmartContext()
        {
            var init = new TruckSmartDBInitializer();
            init.InitializeDatabase(new TruckSmartContext());
        }
        #endregion

        #region Context object initialization
        public TruckSmartContext():base("name=TruckSmartDB")
        {

        }
        public TruckSmartContext(string connection) : base(connection)
        {

        }
        #endregion

        #region Data collection properties.  Core to Entity Framework
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Shipment> Shipments { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<ServiceProvider> ServiceProviders { get; set; }
        #endregion

        #region Shipment/trip management
        public List<Shipment> GetOpenShipments()
        {
            return Shipments.Include(s => s.Driver).Include(s => s.From).Include(s => s.To).Where(s => s.Driver == null).ToList();
        }
        public List<Shipment> GetMyShipments()
        {
            return Shipments.Include(s => s.Driver).Include(s => s.From).Include(s => s.To).Where(s => (s.Driver != null) && (s.Driver.DriverID == this.driverID)).ToList();

        }
        public Shipment GetShipment(Guid id)
        {
            return Shipments.Include(s => s.Driver).Include(s => s.From).Include(s => s.To).Where(s => s.ShipmentID == id).First();
        }
        public Shipment ReserveShipment(Guid id)
        {
            var shipment = Shipments.Include(s => s.Driver).Where(s => s.ShipmentID == id).First();
            //Check to make sure it is not already reserved
            if(shipment.Driver!=null)
            {
                throw new InvalidOperationException("This shipment is already reserved");
            }
            var driver = Drivers.First(d => d.DriverID == this.driverID);
            shipment.Driver = driver;
            SaveChanges();
            return shipment;

        }
        public Shipment ReleaseShipment(Guid id)
        {
            var shipment = Shipments.Include(s => s.Driver).Include(s => s.From).Include(s => s.To).Where(s => s.ShipmentID == id).First();
            if((shipment.Driver == null) || (shipment.Driver.DriverID != this.driverID))
            {
                throw new InvalidOperationException("This shipment is not reserved for the current driver.");
            }
            shipment.Driver = null;
            SaveChanges();
            return shipment;

        }
        #endregion

        #region Provider management
        public List<ServiceProvider> GetProviders()
        {
            //Simplified code for using redis cache
            //List<ServiceProvider> results = null;
            //string cacheKey = "TruckSmart_Providers";
            //IDatabase cache = Connection.GetDatabase();
            //string cacheData = cache.StringGet(cacheKey);
            //if(!string.IsNullOrEmpty(cacheData))
            //{
            //    try
            //    {
            //        results = JsonConvert.DeserializeObject<List<ServiceProvider>>(cacheData);
            //    } catch
            //    {
            //        //Do something if there is an error
            //    }
            //}
            //if(results==null)
            //{
            //    results = this.ServiceProviders.ToList();
            //    cache.StringSet(cacheKey, JsonConvert.SerializeObject(results));
            //}
            List<ServiceProvider> results = this.ServiceProviders.ToList();
            return results;
        }
        public ServiceProvider GetNearestProvider(double latitude, double longitude)
        {
            /*
            Theorectically this would do geo-based calculations to return the closest provider.
            At the moment it returns a random result.  This would be bad for an actual driver,
            but it serves our purposes here.
            */
            var providers = GetProviders();
            var id = (int) Math.Truncate(((new Random()).NextDouble() * (double)providers.Count));
            return providers[id];
        }
        #endregion

        #region Expense management
        //Note: Expenses are not saved to the relational database
        public List<Expense> GetExpenses(Guid? ShipmentID = null, DateTime? From = null, DateTime? To = null)
        {
            var expenses = new List<Expense>();


            return expenses;
        }
        public Expense GetExpense(Guid ExpenseID)
        {
            var expense = new Expense();


            return expense;
        }
        public Expense SaveExpense(Expense NewExpense, byte[] receipt)
        {
            //Add code to save an expense record and receipt image
            return NewExpense;
        }

        /// <summary>
        /// The generateExpenses method just dumps a bunch of random expenses into the expense table.
        /// </summary>
        private void generateExpenses()
        {

            string[] hotels = { "Marriot", "Hyatt", "Westin", "Motel 6", "Days Inn" };
            string[] locations = { "Pacific Coast Highway", "Jersey Turnpike", "Route 66", "Miracle Mile", "Chisolm Trail" };
            var rnd = new Random();
            foreach(var shipment in this.GetMyShipments())
            {
                for(int lcv = 0; lcv<5; lcv++)
                {
                    SaveExpense(new Expense
                    {
                        ExpenseID = Guid.NewGuid(),
                        DriverID = this.driverID,
                        ShipmentID = shipment.ShipmentID,
                        ExpenseType = ExpenseTypeEnum.Lodging,
                        Date = shipment.Scheduled.AddDays(-lcv),
                        Amount = (decimal)(100f + 1000f * rnd.NextDouble()),
                        HasReceipt = false,
                        ReceiptURL = "",
                        Hotel = hotels[lcv],
                        Room = (lcv * 100 + 15).ToString(),
                        DirectBill = lcv % 3 == 0
                        
                    },null);
                    SaveExpense(new Expense
                    {
                        ExpenseID = Guid.NewGuid(),
                        DriverID = this.driverID,
                        ShipmentID = shipment.ShipmentID,
                        ExpenseType = ExpenseTypeEnum.Toll,
                        Date = shipment.Scheduled.AddDays(-lcv),
                        Amount = (decimal)(.25f + 10f * rnd.NextDouble()),
                        HasReceipt = false,
                        ReceiptURL = "",
                        Location = locations[lcv]
                    },null);
                }
            }
            
        }

        #endregion

    }
}
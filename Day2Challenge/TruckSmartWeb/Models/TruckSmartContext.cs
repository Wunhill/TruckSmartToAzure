using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TruckSmartWeb.Models
{
    public class TruckSmartContext : DbContext
    {
        #region Database context configuration
        //static TruckSmartContext()
        //{
        //    var init = new TruckSmartDBInitializer();
        //    init.InitializeDatabase(new TruckSmartContext());
        //}
<<<<<<< HEAD

        #region Standard initialization
        public TruckSmartContext():base("name=TruckSmartDB")
=======
        public TruckSmartContext() : base("name=TruckSmartDB")
>>>>>>> master
        {

        }
        public TruckSmartContext(string connection) : base(connection)
        {

        }
        #endregion

        #region DBSet properties
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Contractor> Contractors { get; set; }
        public DbSet<Shipment> Shipments { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<ServiceProvider> ServiceProviders { get; set; }
        #endregion

        #region Shipment management
        public List<Shipment> GetOpenShipments()
        {
            return Shipments.Include(s => s.Driver).Include(s => s.From).Include(s => s.To).Where(s => s.Driver == null).ToList();
        }
        public List<Shipment> GetMyShipments()
        {
            return Shipments.Include(s => s.Driver).Include(s => s.From).Include(s => s.To).Where(s => (s.Driver != null) && (s.Driver.ContractorID == WebApiApplication.CurrentUser)).ToList();

        }
        public Shipment GetShipment(Guid id)
        {
            return Shipments.Include(s => s.Driver).Include(s => s.From).Include(s => s.To).Where(s => s.ShipmentID == id).First();
        }
        public Shipment ReserveShipment(Guid id)
        {
            var shipment = Shipments.Include(s => s.Driver).Where(s => s.ShipmentID == id).First();
            //Check to make sure it is not already reserved
            if (shipment.Driver != null)
            {
                throw new InvalidOperationException("This shipment is already reserved");
            }
            var driver = Contractors.First(d => d.ContractorID == WebApiApplication.CurrentUser);
            shipment.Driver = driver;
            SaveChanges();
            return shipment;

        }
        public Shipment ReleaseShipment(Guid id)
        {
            var shipment = Shipments.Include(s => s.Driver).Include(s => s.From).Include(s => s.To).Where(s => s.ShipmentID == id).First();
            if ((shipment.Driver == null) || (shipment.Driver.ContractorID != WebApiApplication.CurrentUser))
            {
                throw new InvalidOperationException("This shipment is not reserved for the current driver.");
            }
            shipment.Driver = null;
            SaveChanges();
            return shipment;

        }
        #endregion

        #region Emergency provider management
        public List<ServiceProvider> GetProviders()
        {
            return this.ServiceProviders.ToList();
        }
        public ServiceProvider GetNearestProvider(double latitude, double longitude)
        {
            /*
            Theorectically this would do geo-based calculations to return the closest provider.
            At the moment it returns a random result.  This would be bad for an actual driver,
            but it serves our purposes here.
            */
            var providers = GetProviders();
            var id = (int)Math.Truncate(((new Random()).NextDouble() * (double)providers.Count));
            return providers[id];
        }
        #endregion

    }
}
﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TruckSmartWeb.Models
{
    public class TruckSmartContextOld : DbContext
    {
        #region Database context configuration
        //static TruckSmartContext()
        //{
        //    var init = new TruckSmartDBInitializer();
        //    init.InitializeDatabase(new TruckSmartContext());
        //}
        #endregion

        #region Standard initialization

        public TruckSmartContextOld() : base("name=TruckSmartDB")

        {

        }
        public TruckSmartContextOld(string connection) : base(connection)
        {

        }
        #endregion

        #region DBSet properties
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Shipment> Shipments { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<ServiceProvider> ServiceProviders { get; set; }
        #endregion
        private string driverID
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["testDriverID"];
            }
        }

        #region Shipment management
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
            if (shipment.Driver != null)
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
            if ((shipment.Driver == null) || (shipment.Driver.DriverID != this.driverID))
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TruckSmartWeb.Models;

namespace TruckSmartWeb.Controllers
{
    
    public class DriverController : Controller
    {
        // GET: Driver
        TruckSmartContext context = new TruckSmartContext();
        public ActionResult Index()
        {
            var driverData = new DriverData { MyShipments = context.GetMyShipments(), OpenShipments = context.GetOpenShipments()};
            return View(driverData);
        }
        public ActionResult DisplayShipment(Guid id)
        {
            
            return View(context.GetShipment(id));
        }

        public ActionResult ReserveShipment(Guid id)
        {
            return View(context.GetShipment(id));
        }
        [HttpPost]
        public ActionResult ReserveShipment(Guid id, bool Reserve)
        {
            context.ReserveShipment(id);
            return RedirectToAction("Index");
        }
        public ActionResult ReleaseShipment(Guid id)
        {
            return View(context.GetShipment(id));
        }
        [HttpPost]
        public ActionResult ReleaseShipment(Guid id, bool Release)
        {
            context.ReleaseShipment(id);
            return RedirectToAction("Index");
        }
        public ActionResult Info()
        {
            return View();
        }
    }

    public class DriverData
    {
        public List<Shipment> MyShipments { get; set; }
        public List<Shipment> OpenShipments { get; set; }
    }

}
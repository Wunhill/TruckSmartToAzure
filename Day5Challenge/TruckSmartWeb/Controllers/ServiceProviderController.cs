using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TruckSmartWeb.Models;

namespace TruckSmartWeb.Controllers
{
    public class ServiceProviderController : Controller
    {
        // GET: ServiceProvider
        private TruckSmartContext context = new TruckSmartContext();
        public ActionResult Index()
        {
            return View(context.GetProviders());
        }

        public ActionResult GetProvider(double latitude, double longitude)
        {
            return View(context.GetNearestProvider(latitude, longitude));
        }
    }
}
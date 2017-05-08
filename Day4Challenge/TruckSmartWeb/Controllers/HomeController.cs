using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TruckSmartWeb.Models;

namespace TruckSmartWeb.Controllers
{
    public class HomeController : Controller
    {
        TruckSmartContext ct = new TruckSmartContext();
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
    }
}

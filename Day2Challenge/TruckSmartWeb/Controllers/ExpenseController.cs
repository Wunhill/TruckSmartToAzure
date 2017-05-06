using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TruckSmartWeb.Controllers
{
    public class ExpenseController : Controller
    {
        // GET: Expenses
        public ActionResult Index()
        {
            return View();
        }
    }
}
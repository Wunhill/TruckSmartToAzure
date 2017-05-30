using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TruckSmartWeb.Models;

namespace TruckSmartWeb.Controllers
{
    public class ExpenseController : Controller
    {
        // GET: Expenses
        private TruckSmartContext context = new TruckSmartContext();
        public ExpenseController() : base()
        {
            ViewBag.Shipments = context.GetMyShipments();
        }
        public ActionResult Index()
        {
            return View(context.GetExpenses());
        }
        public ActionResult Details(Guid ExpenseID)
        {
            return View(context.GetExpense(ExpenseID));
        }

        [HttpPost]
        public PartialViewResult FilterByDate(string From, string To)
        {
            //This is a terrible way to do this, but it works okay.
            DateTime dTo = DateTime.MaxValue;
            DateTime dFrom = DateTime.MinValue;
            DateTime.TryParse(To, out dTo);
            DateTime.TryParse(From,out dFrom);
            return PartialView("ExpenseList",context.GetExpenses(From: dFrom, To: dTo));
        }
        [HttpPost]
        public PartialViewResult FilterByShipment(string ShipmentID)
        {
            //Also terrible, but not the point of the exercise.
            Guid gShipmentID = Guid.NewGuid();
            Guid.TryParse(ShipmentID, out gShipmentID);

            return PartialView("ExpenseList", context.GetExpenses(ShipmentID:gShipmentID));
        }

        public ActionResult Create()
        {
            return View(new Expense
            {
                DriverID = Session["DriverID"].ToString()
            });
        }
        [HttpPost]
        public ActionResult Create(Expense expense, HttpPostedFileBase file)
        {
            if(ModelState.IsValid)
            {
                byte[] fileBytes = null;
                expense.HasReceipt = file != null;
                if (file != null)
                {
                    fileBytes = new byte[file.ContentLength];
                    file.InputStream.Read(fileBytes, 0, file.ContentLength);
                }
                context.SaveExpense(expense, fileBytes);
                return RedirectToAction("Index");
            }

            ViewBag.Shipments = context.GetMyShipments();

            return View(expense);
        }

    }
}
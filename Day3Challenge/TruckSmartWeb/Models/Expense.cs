using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace TruckSmartWeb.Models
{
    public class Expense
    {
        public Guid ExpenseID { get; set; }
        public ExpenseTypeEnum ExpenseType { get; set; }
        public string DriverID { get; set; }
        [DisplayName("Trip")]
        public Guid ShipmentID { get; set; }
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }
        public bool HasReceipt { get; set; }
        public string ReceiptURL { get; set; }
        public string Location { get; set; }
        public string Hotel { get; set; }
        public string Room { get; set; }
        public bool DirectBill { get; set; }
    }
    public enum ExpenseTypeEnum
    {
        Toll,
        Lodging
    }

    public class ExpenseSubmit:Expense
    {
        public byte[] File { get; set; }
    }

}
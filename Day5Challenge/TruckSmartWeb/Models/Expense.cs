using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.WindowsAzure.Storage.Table;

namespace TruckSmartWeb.Models
{
    public class Expense : TableEntity
    {
        public Guid ExpenseID
        {
            get
            {
                if(string.IsNullOrEmpty(this.RowKey))
                {
                    this.RowKey = Guid.NewGuid().ToString();
                }
                return new Guid(this.RowKey);
            }
            set
            {
                this.RowKey = value.ToString();
            }
        }
        public int ExpenseTypeValue { get; set; }
        public ExpenseTypeEnum ExpenseType
        {
            get
            {
                return (ExpenseTypeEnum)ExpenseTypeValue;
            }
            set
            {
                ExpenseTypeValue = (int)value;
            }
        }
        public string DriverID
        {
            get
            {
                return this.PartitionKey;
            }
            set
            {
                this.PartitionKey = value;
            }
        }
        [DisplayName("Trip")]
        public Guid ShipmentID { get; set; }
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        [DataType(DataType.Currency)]
        public double Amount { get; set; }
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

    public class ExpenseSubmit : Expense
    {
        public byte[] File { get; set; }
    }

}
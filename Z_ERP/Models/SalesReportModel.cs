using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Z_ERP.Models
{
    public class SalesReportModel
    {
 
 
 
 
 

        public string ItemName { get; set; }
        public string PointOfSaleName { get; set; }
       
        public string RecieptNo  { get; set; } 
        public decimal ItemSalePrice { get; set; } 
        public decimal SaleQuantity { get; set; }
        public decimal ItemTotalSaleAmount { get; set; }
        public Boolean SaleIsReturned { get; set; }
        public DateTime SaleReturnedDate { get; set; }
        public DateTime SaleDate { get; set; }
    }
}
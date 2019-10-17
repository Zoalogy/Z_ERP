using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Z_ERP.Models
{
    public class TrucksReportModel
    {

        public string ExpenseName { get; set; }
        public string ExpenseAmount { get; set; }

        public string TripName { get; set; }

        public string ExpenseDate { get; set; }
        public string TruckNameAr { get; internal set; }
    }
   

}
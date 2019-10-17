using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Z_ERP.Models
{
    public class DashboardClass
    {
         
        public List<string >   ChartLabel { get; set; }
        public List<decimal >  ChartData { get; set; }

        public DashboardClass()
        {
            ChartData = new List<decimal>();
            ChartLabel = new List<string>();
        }

    }
    public class DashboardCardClass
    {

        public  string YeasterDayAmount  { get; set; }
        public string CustomerCreditAmount { get; set; }

        public string ExpiredItems { get; set; }

        public string InventroyRequests { get; set; }
        

         
        

    }

    public class DashboardModel
    {
        public DashboardClass ChartData { set; get; }
        public int chartID { get; set; }
        public string  CardValue { get; set; }
        public DashboardModel()
        {
            ChartData = new DashboardClass();
        }
    }


}
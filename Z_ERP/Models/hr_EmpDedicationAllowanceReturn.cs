using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Z_ERP.Models
{
        public  class hr_EmpDedicationAllowanceReturn
    {
        
        public int EmpDedicationAllowanceID { get; set; }

        public string DedicationAllowanceListName { get; set; }

        public string EmployeeName { get; set; }

        public string EmpBatchValue { get; set; }

        
        public string EmpBatchDate { get; set; }
    }
}

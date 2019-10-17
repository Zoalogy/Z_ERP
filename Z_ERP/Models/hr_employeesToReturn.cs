using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Z_ERP.Models
{
    public class hr_employeesToReturn
    {
        public int EmployeeID { get; set; }
        public string EmployeeFullName { get; set; }
        public string EmployeeUserName { get; set; }
        public string EmployeePassword { get; set; }
        public string EmployeeAddress { get; set; }
        public string EmployeeAcountNo { get; set; }
        public string EmployeeBankBranch { get; set; }
        public string EmployeeGender { get; set; }
        public string EmployeeMaritelStatus { get; set; }
        public string EmployeeDepartmentID { get; set; }
        public string EmployeeJobID { get; set; }
        public string EmployeeStatus { get; set; }
        public Nullable<System.DateTime> EmployeeStatusDate { get; set; }
        public Nullable<System.DateTime> RegisterationDate { get; set; }
        public Nullable<System.DateTime> EmployeeLastLogin { get; set; }
        public string EmployeeEmail { get; set; }
        public string EmployeePhone { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Z_ERP.Models
{
    public class hr_EmployeesSalaryReturn
    {
        public int Id { get; set; }


        public String EmployeeName { get; set; }
        public Double Allownaces   { get; set; } 
        
        public float EmployeesBasicSalary { get; set; }
        public Double Deducations { get; set; }
        public Double EmployeesSalary { get; set; }
        public Double EmployeesExpenses { get; set; }

        public DateTime? EmployeesSalarydate { get; set; }
    }
}
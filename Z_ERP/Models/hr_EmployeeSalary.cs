namespace Z_ERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class hr_EmployeeSalary
    {
        public int Id { get; set; }

        public int? EmployeeID { get; set; }

        public double? EmployeesSalary { get; set; }

        public bool Uploaded { get; set; }

        public double EmployeesSalaryAllownace { get; set; }

        public double EmployeesSalarydeducate { get; set; }

        [Column(TypeName = "date")]
        public DateTime EmployeesSalarydate { get; set; }
    }
}

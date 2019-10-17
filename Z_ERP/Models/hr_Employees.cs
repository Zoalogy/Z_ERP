namespace Z_ERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class hr_Employees
    {
        [Key]
        public int EmployeeID { get; set; }

        [StringLength(100)]
        public string EmployeeFullName { get; set; }

        [StringLength(100)]
        public string EmployeeUserName { get; set; }

        [StringLength(250)]
        public string EmployeePassword { get; set; }

        [StringLength(100)]
        public string EmployeeAddress { get; set; }

        [StringLength(100)]
        public string EmployeeAcountNo { get; set; }

        [StringLength(100)]
        public string EmployeeBankBranch { get; set; }

        public int? EmployeeGender { get; set; }

        public int? PointOfsaleID { get; set; }

        public bool Uploaded { get; set; }

        [StringLength(100)]
        public string EmployeeMaritelStatus { get; set; }

        public int? EmployeeDepartmentID { get; set; }

        public int? EmployeeJobID { get; set; }

        public int? EmployeeStatus { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EmployeeStatusDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? RegisterationDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EmployeeLastLogin { get; set; }

        [StringLength(100)]
        public string EmployeeEmail { get; set; }

        [StringLength(100)]
        public string EmployeePhone { get; set; }
    }
}

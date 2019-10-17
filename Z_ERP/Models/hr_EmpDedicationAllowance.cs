namespace Z_ERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class hr_EmpDedicationAllowance
    {
        [Key]
        public int EmpDedicationAllowanceID { get; set; }

        public int? DedicationAllowanceListID { get; set; }

        public int? EmployeeID { get; set; }

        public bool Uploaded { get; set; }

        public decimal? EmpBatchValue { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EmpBatchDate { get; set; }
    }
}

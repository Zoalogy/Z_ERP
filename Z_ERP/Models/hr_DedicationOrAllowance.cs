namespace Z_ERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class hr_DedicationOrAllowance
    {
        [Key]
        public int DedicationOrAllowanceID { get; set; }

        [StringLength(100)]
        public string DedicationOrAllowanceNameAr { get; set; }

        public bool Uploaded { get; set; }

        [StringLength(100)]
        public string DedicationOrAllowanceNameEn { get; set; }
    }
}

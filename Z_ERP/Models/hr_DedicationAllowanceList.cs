namespace Z_ERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class hr_DedicationAllowanceList
    {
        [Key]
        public int DeductionAllowanceListID { get; set; }

        public int? DeductionAllowanceID { get; set; }

        public bool Uploaded { get; set; }

        [StringLength(100)]
        public string DeductionAllowanceNmaeAr { get; set; }

        [StringLength(100)]
        public string DeductionAllowanceNmaeEn { get; set; }
    }
}

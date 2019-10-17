namespace Z_ERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class hr_JobsName
    {
        [Key]
        public int JobNameID { get; set; }

        public int? DepartmentID { get; set; }

        public bool Uploaded { get; set; }

        [StringLength(100)]
        public string JobNameAr { get; set; }

        [StringLength(100)]
        public string JobNameEn { get; set; }

        public decimal? JobNameBasicSalary { get; set; }
    }
}

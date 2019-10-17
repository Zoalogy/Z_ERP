namespace Z_ERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class hr_EmployeeSatus
    {
        [Key]
        public int EmployeeSatusId { get; set; }

        [StringLength(50)]
        public string EmployeeSatusAr { get; set; }

        public bool Uploaded { get; set; }

        [StringLength(50)]
        public string EmployeeSatusEn { get; set; }
    }
}

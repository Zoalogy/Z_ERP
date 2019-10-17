namespace Z_ERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class hr_Department
    {
        [Key]
        public int DepartmentID { get; set; }

        [StringLength(100)]
        public string DepartmentNameEn { get; set; }

        [StringLength(100)]
        public string DepartmentNameAr { get; set; }

        public bool Uploaded { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DepartmentDate { get; set; }
    }
}

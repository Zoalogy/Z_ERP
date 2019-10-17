namespace Z_ERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class hr_SalaryHistory
    {
        [Key]
        public int SalaryHistoryID { get; set; }

        public int ImployeeID { get; set; }

        [Required]
        [StringLength(250)]
        public string ImployeeName { get; set; }

        public double SalaryHistoryAllownaceAmount { get; set; }

        public double SalaryHistoryDeducationAmount { get; set; }

        public double SalaryHistoryAmount { get; set; }

        [Column(TypeName = "date")]
        public DateTime SalaryHistoryDate { get; set; }

        public bool UpLoaded { get; set; }

        [Column(TypeName = "date")]
        public DateTime SalaryHistoryProcduerDate { get; set; }
    }
}

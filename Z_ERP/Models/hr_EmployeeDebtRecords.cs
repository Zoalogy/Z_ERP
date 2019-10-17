namespace Z_ERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class hr_EmployeeDebtRecords
    {
        [Key]
        public int DebtRecordsID { get; set; }

        public int? DebtRecordsEmpoloyeeID { get; set; }

        [StringLength(50)]
        public string DebtRecordsEmployeeName { get; set; }

        [StringLength(50)]
        public string AdminName { get; set; }

        public decimal? DebtRecordsAmount { get; set; }

        public string DebtRecordsDescription { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EmployeeDebtRecordsDate { get; set; }
    }
}

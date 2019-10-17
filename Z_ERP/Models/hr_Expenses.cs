namespace Z_ERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class hr_Expenses
    {
        
        [Key]
        public int ExpensesID { get; set; }

        public decimal? ExpensesAmount { get; set; }

        public string ExpensesDescription { get; set; }

        [StringLength(250)]
        public string EmployeeName { get; set; }

        public bool Uploaded { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ExpensesDate { get; set; }

        public int? PointOfSaleID { get; set; }
    }
}

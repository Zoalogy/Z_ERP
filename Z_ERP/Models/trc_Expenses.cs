namespace Z_ERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class trc_Expenses
    {
        [Key]
        public int ExpenseID { get; set; }

        [StringLength(200)]
        public string ExpenseName { get; set; }

        [StringLength(200)]
        public string ExpenseDescription { get; set; }

        public decimal? ExpenseAmount { get; set; }

        public int? TripID { get; set; }

        public int? TruckID { get; set; }

        public bool? UpLoaded { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ExpenseDate { get; set; }
    }
}

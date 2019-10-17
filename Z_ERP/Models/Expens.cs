namespace Z_ERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Expenses")]
    public partial class Expens
    {
        [Key]
        public int ExpensesID { get; set; }

        public decimal? ExpensesAmount { get; set; }

        public string ExpensesDescription { get; set; }

        [StringLength(250)]
        public string EmployeeName { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ExpensesDate { get; set; }
    }
}

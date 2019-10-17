namespace Z_ERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sal_Customer
    {
        [Key]
        public int CustomerID { get; set; }

        [StringLength(100)]
        public string CustomerName { get; set; }

        [StringLength(100)]
        public string CustomerPhone { get; set; }

        [StringLength(100)]
        public string CustomerEmail { get; set; }

        [StringLength(100)]
        public string CustomerBankAcount { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CustomerRegisterationDate { get; set; }

        public bool? UpLoaded { get; set; }
    }
}

namespace Z_ERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class pay_CustomerBankAcounts
    {
        [Key]
        public int CustomerBankAcountID { get; set; }

        public int? CustomerID { get; set; }

        public decimal? CustomerBankAcountDepit { get; set; }

        public decimal? CustomerBankAcountCredit { get; set; }

        public decimal? CustomerBankAcountAmount { get; set; }

        [StringLength(200)]
        public string PaymentDescription { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CustomerBankAcountTransationDate { get; set; }

        public bool? UpLoaded { get; set; }
    }
}

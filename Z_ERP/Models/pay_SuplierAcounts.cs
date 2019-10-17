namespace Z_ERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class pay_SuplierAcounts
    {
        [Key]
        public int SuplierAcountID { get; set; }

        public int? SuplierID { get; set; }

        [StringLength(200)]
        public string SuplierName { get; set; }

        public decimal? SuplierAcountDepit { get; set; }

        public decimal? SuplierAcountCredit { get; set; }

        public decimal? SuplierAcountAmount { get; set; }

        [StringLength(200)]
        public string PaymentDescription { get; set; }

        [Column(TypeName = "date")]
        public DateTime? SuplierAcountTransactionDate { get; set; }

        public int? PaymentMethodID { get; set; }

        public bool? UpLoaded { get; set; }
    }
}

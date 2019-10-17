namespace Z_ERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class pay_CustomerAcounts
    {
        [Key]
        public int CustomerAcountID { get; set; }

        public int? CustomerID { get; set; }

        [StringLength(200)]
        public string CustomerName { get; set; }

        public decimal? CustomerAcountDepit { get; set; }

        public decimal? CustomerAcountCredit { get; set; }

        public decimal? CustomerAcountAmount { get; set; }

        [StringLength(200)]
        public string PaymentDescription { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CustomerAcountTransactionDate { get; set; }

        public int? PaymentMethodID { get; set; }

        public bool? UpLoaded { get; set; }

        public int? PointOfSaleID { get; set; }
    }
}

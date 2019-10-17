namespace Z_ERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class pay_Payment
    {
        [Key]
        public int PaymentID { get; set; }

        public decimal? PaymentDepitAmount { get; set; }

        public decimal? PaymentCreditAmount { get; set; }

        public decimal? PaymentBalance { get; set; }

        [StringLength(200)]
        public string PaymentDescription { get; set; }

        [Column(TypeName = "date")]
        public DateTime? PaymentDate { get; set; }

        public bool? UpLoaded { get; set; }

        public int? PaymentMethodID { get; set; }

        public int? PointOfSaleID { get; set; }
    }
}

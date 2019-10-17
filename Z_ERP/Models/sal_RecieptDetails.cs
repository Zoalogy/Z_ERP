namespace Z_ERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sal_RecieptDetails
    {
        [Key]
        public int RecieptDetailsID { get; set; }

        [StringLength(100)]
        public string RecieptNo { get; set; }

        public decimal? Amount { get; set; }

        [StringLength(200)]
        public string RecieptDetailsDescription { get; set; }

        public int? PaymentMethodID { get; set; }

        [StringLength(200)]
        public string ChequeNo { get; set; }

        public DateTime? ChequeDate { get; set; }

        [StringLength(200)]
        public string ChequeBank { get; set; }

        [StringLength(200)]
        public string ChequeBankBranch { get; set; }

        [Column(TypeName = "date")]
        public DateTime? RecieptDetailsDate { get; set; }

        public bool? UpLoaded { get; set; }
    }
}

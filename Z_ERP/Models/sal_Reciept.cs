namespace Z_ERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sal_Reciept
    {
        [Key]
        public int RecieptID { get; set; }

        [StringLength(100)]
        public string RecieptNo { get; set; }

        public int? PaymentMethodID { get; set; }

        public int? CustomerID { get; set; }

        [StringLength(200)]
        public string CustomerName { get; set; }

        public decimal? RecieptTotalAmount { get; set; }

        public decimal? RecieptDiscount { get; set; }

        public decimal? RecieptRemaining { get; set; }

        public decimal? RecieptTaxes { get; set; }

        public decimal? RecieptNetAmount { get; set; }

        public decimal? RecieptPaidAmount { get; set; }

        public string RecieptDescription { get; set; }

        public bool? WithInstallments { get; set; }

        public int? InstallmentsNo { get; set; }

        public int? RecieptPaymentStatus { get; set; }

        public bool? RecieptIsReturned { get; set; }

        [Column(TypeName = "date")]
        public DateTime? RecieptReturnDate { get; set; }

        public bool? UpLoaded { get; set; }

        [Column(TypeName = "date")]
        public DateTime? RecieptDate { get; set; }

        public int? PointOfSaleID { get; set; }
    }
}

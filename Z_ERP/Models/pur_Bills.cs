namespace Z_ERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class pur_Bills
    {
        [Key]
        public int BillID { get; set; }

        public int? PointOfSaleID { get; set; }

        public decimal? TotalCostPurchase { get; set; }

        [StringLength(100)]
        public string BillNo { get; set; }

        [Column("BillStatusID ")]
        public int? BillStatusID { get; set; }

        public int? SuplierID { get; set; }

        [StringLength(200)]
        public string SuplierName { get; set; }

        public decimal? TotalAmount { get; set; }

        public decimal? BillTotalAmount { get; set; }

        [StringLength(10)]
        public string BillTaxes { get; set; }

        public decimal? BillNetAmount { get; set; }

        public decimal? BillPaidAmount { get; set; }

        public decimal? BillRemainingAmount { get; set; }

        [StringLength(200)]
        public string BillDescription { get; set; }

        public int? BillPaymentStatus { get; set; }

        [Column(TypeName = "date")]
        public DateTime? BillDate { get; set; }

        public bool? UpLoaded { get; set; }

        public int? PaymentMethodID { get; set; }

        [Column("BillIsReturned ")]
        public bool BillIsReturned { get; set; }

        [Column("BillReturnDate ")]
        public DateTime? BillReturnDate { get; set; }
    }
}

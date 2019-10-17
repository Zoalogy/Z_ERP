namespace Z_ERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class pur_Purchase
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PurchaseItemID { get; set; }

        public int? PointOfSaleID { get; set; }

        [StringLength(100)]
        public string PurchaseItemNameAr { get; set; }

        [StringLength(100)]
        public string PurchaseItemNameEn { get; set; }

        public int? PurchaseItemQuantity { get; set; }

        public decimal? ItemPurchasePrice { get; set; }

        public decimal? ItemCostPurchase { get; set; }

        public decimal? ItemTotalPurchaseAmount { get; set; }

        public int? ItemPurchaseCurrencyID { get; set; }

        public decimal? ItemSalePrice { get; set; }

        public int? ItemSaleCurrencyID { get; set; }

        public int? CategoryID { get; set; }

        public int? SuplierID { get; set; }

        [StringLength(100)]
        public string SuplierName { get; set; }

        public int? SubCategoryID { get; set; }

        public int? ItemID { get; set; }

        [StringLength(100)]
        public string BillNo { get; set; }

        public int? PurchaseStatusID { get; set; }

        [Column(TypeName = "date")]
        public DateTime? PurchaseItemAddedDate { get; set; }

        [Key]
        [Column(Order = 1)]
        public bool IsReturn { get; set; }

        public bool? UpLoaded { get; set; }

        [StringLength(200)]
        public string BarCode { get; set; }
    }
}

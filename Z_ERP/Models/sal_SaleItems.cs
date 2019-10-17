namespace Z_ERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sal_SaleItems
    {
        [Key]
        public int SaleItemsID { get; set; }

        public int PointOfSaleID { get; set; }

        public int ItemID { get; set; }

        [StringLength(100)]
        public string SaleItemsNameAr { get; set; }

        [StringLength(100)]
        public string SaleItemsNameEn { get; set; }

        public long? SaleItemsQuantity { get; set; }

        public int? SaleItemsMinimumQuantity { get; set; }

        public decimal? ItemSalePrice { get; set; }

        public int? ItemSaleCurrencyID { get; set; }

        public decimal? ItemPurchasePrice { get; set; }

        public int? ItemPurchaseCurrencyID { get; set; }

        public int? CategoryID { get; set; }

        public int? SubCategoryID { get; set; }

        [Column(TypeName = "date")]
        public DateTime? SaleItemsExpiredDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? SaleItemsAddedDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? SaleItemsUpdatedAt { get; set; }

        public bool? UpLoaded { get; set; }
    }
}

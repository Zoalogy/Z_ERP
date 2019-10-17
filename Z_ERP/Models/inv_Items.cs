namespace Z_ERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class inv_Items
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ItemID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string ItemBatch { get; set; }

        [StringLength(100)]
        public string ItemNameAr { get; set; }

        [StringLength(100)]
        public string ItemNameEn { get; set; }

        public int ItemQuantity { get; set; }

        public int ItemMinimumQuantity { get; set; }

        public decimal? ItemSalePrice { get; set; }

        public decimal? ItemSaleCurrencyID { get; set; }

        public decimal? ItemPuchasePrice { get; set; }

        public int? ItemPurchaseCurrencyID { get; set; }

        public int InventoryID { get; set; }

        public int CategoryID { get; set; }

        public int SubCategoryID { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ItemExpiredDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ItemAddedDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ItemUpdatedAt { get; set; }

        public bool? UpLoaded { get; set; }

        [StringLength(200)]
        public string BarCode { get; set; }
    }
}

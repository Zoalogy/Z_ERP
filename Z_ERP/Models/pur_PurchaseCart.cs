namespace Z_ERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class pur_PurchaseCart
    {
        [Key]
        public int purchaseCartID { get; set; }

        public int? ItemID { get; set; }

        public int? ItemInventoryID { get; set; }

        public int? ItemCategoryID { get; set; }

        [StringLength(200)]
        public string ItemName { get; set; }

        public int? ItemQuantity { get; set; }

        public decimal? ItempurchasePrice { get; set; }

        public decimal? ItemSellPrice { get; set; }

        public decimal? ItemCostPurchase { get; set; }

        [StringLength(200)]
        public string UserName { get; set; }

        [Column(TypeName = "date")]
        public DateTime? PurchaseCartDate { get; set; }
    }
}

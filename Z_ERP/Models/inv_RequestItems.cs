namespace Z_ERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class inv_RequestItems
    {
        [Key]
        public int RequestItemID { get; set; }

        [StringLength(100)]
        public string RequestNo { get; set; }

        public int ItemID { get; set; }

        [StringLength(100)]
        public string ItemNameAr { get; set; }

        [StringLength(100)]
        public string ItemNameEn { get; set; }

        public decimal? ItemSalePrice { get; set; }

        public decimal? ItemPurshasePrice { get; set; }

        public decimal? ItemQuantity { get; set; }

        public decimal? ItemTotalAmount { get; set; }

        public int? ItemCurrencyID { get; set; }

        [Column(TypeName = "date")]
        public DateTime? RequestDate { get; set; }

        public int? CategoryID { get; set; }

        public int? SubCategoryID { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ItemExpiredDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ItemAddedDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ItemUpdatedAt { get; set; }

        public bool? UpLoaded { get; set; }
    }
}

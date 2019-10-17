namespace Z_ERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sal_Sales
    {
        [Key]
        public int SaleID { get; set; }

        public decimal? SalePrice { get; set; }

        [StringLength(100)]
        public string RecieptNo { get; set; }

        public int? SalesItemsID { get; set; }

        [StringLength(200)]
        public string ItemName { get; set; }

        public decimal? ItemSalePrice { get; set; }

        public decimal? ItemPurchasePrice { get; set; }

        public long? SaleQuantity { get; set; }

        public decimal? ItemTotalSaleAmount { get; set; }

        public bool? SaleIsReturned { get; set; }

        [Column(TypeName = "date")]
        public DateTime? SaleReturnedDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? SaleDate { get; set; }

        public int? PointOfSaleID { get; set; }
      

        

        public bool? UpLoaded { get; set; }
    }
}

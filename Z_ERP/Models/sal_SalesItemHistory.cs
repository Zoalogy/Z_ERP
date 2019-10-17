namespace Z_ERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sal_SalesItemHistory
    {
        [Key]
        public int ItemHistroyHistroyID { get; set; }

        public int SaleItemID { get; set; }

        public long SaleItemQuantity { get; set; }

        public bool? SaleItemDebitOrCredit { get; set; }

        public decimal? ItemAmount { get; set; }

        public decimal? ItemCurrentQuantity { get; set; }

        [StringLength(250)]
        public string ItemHistoryDescription { get; set; }

        [StringLength(250)]
        public string EmployeeName { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ItemHistoryDate { get; set; }
       
        public bool? UpLoaded { get; set; }
    }
}

namespace Z_ERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class inv_ItemHistory
    {
        [Key]
        public int ItemHistoryID { get; set; }

        public int? ItemID { get; set; }

        public long? ItemHistoryQuantity { get; set; }

        [StringLength(100)]
        public string QuantityMeasureUnit { get; set; }

        public bool? ItemHistoryDebitOrCredit { get; set; }

        public long? ItemHistoryCuurentQuantity { get; set; }

        public int? ItemHistoryProccessTypeID { get; set; }

        [StringLength(200)]
        public string ItemHistoryProccessType { get; set; }

        [StringLength(200)]
        public string ItemName { get; set; }

        [StringLength(200)]
        public string ItemHistoryDecription { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ItemHistoryDate { get; set; }

        public bool? UpLoaded { get; set; }

        [StringLength(50)]
        public string ItemHistoyPrice { get; set; }
    }
}

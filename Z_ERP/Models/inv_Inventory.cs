namespace Z_ERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class inv_Inventory
    {
        [Key]
        public int InvertoryID { get; set; }

        [StringLength(100)]
        public string InvertoryNameAr { get; set; }

        [StringLength(100)]
        public string InvertoryNameEn { get; set; }

        [StringLength(100)]
        public string InvertoryAddressAr { get; set; }

        [StringLength(100)]
        public string InvertoryAddressEn { get; set; }

        [Column(TypeName = "date")]
        public DateTime? InvertoryAddDate { get; set; }

        public decimal? InvertoryRent { get; set; }

        [Column(TypeName = "date")]
        public DateTime? InvertoryRentDate { get; set; }

        public bool? UpLoaded { get; set; }
    }
}

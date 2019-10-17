namespace Z_ERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sal_pointOfSale
    {
        [Key]
        public int PointOfSaleID { get; set; }

        [StringLength(100)]
        public string PointOfSaleAddress { get; set; }

        [StringLength(100)]
        public string PointOfSalePhone { get; set; }

        [StringLength(100)]
        public string PointOfSaleName { get; set; }

        public bool? UpLoaded { get; set; }

        [Column(TypeName = "date")]
        public DateTime? AddedDate { get; set; }
    }
}

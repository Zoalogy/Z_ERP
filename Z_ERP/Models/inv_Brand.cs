namespace Z_ERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class inv_Brand
    {
        [Key]
        public int BrandID { get; set; }

        [StringLength(100)]
        public string BrandNameAr { get; set; }

        [StringLength(100)]
        public string BrandNameEn { get; set; }
    }
}

namespace Z_ERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class inv_Categories
    {
        [Key]
        public int CategoryID { get; set; }

        [StringLength(100)]
        public string CategoryNameAR { get; set; }

        [StringLength(100)]
        public string CategoryNameEN { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CategoryDate { get; set; }

        public bool? UpLoaded { get; set; }
    }
}

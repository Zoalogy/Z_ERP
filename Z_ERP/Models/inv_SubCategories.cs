namespace Z_ERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class inv_SubCategories
    {
        [Key]
        public int SubCategoryID { get; set; }

        public int? CategoryID { get; set; }

        [StringLength(100)]
        public string SubCategoryNameAr { get; set; }

        [StringLength(100)]
        public string SubCategoryNameEN { get; set; }
    }
}

namespace Z_ERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class co_Branches
    {
        [Key]
        public int BranchID { get; set; }

        [StringLength(100)]
        public string BranchNameAr { get; set; }

        [StringLength(100)]
        public string BranchNameEn { get; set; }

        [StringLength(100)]
        public string BranchAddressAr { get; set; }

        [StringLength(100)]
        public string BranchAddressAEn { get; set; }

        [StringLength(100)]
        public string BranchPhone { get; set; }

        [StringLength(100)]
        public string BranchEmail { get; set; }

        [Column(TypeName = "date")]
        public DateTime? BranchAddDate { get; set; }
    }
}

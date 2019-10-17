namespace Z_ERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sys_Branches
    {
        [Key]
        public int BranchID { get; set; }

        [StringLength(50)]
        public string BranchName { get; set; }

        public bool? isActive { get; set; }
    }
}

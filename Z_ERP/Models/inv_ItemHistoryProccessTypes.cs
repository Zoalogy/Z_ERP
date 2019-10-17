namespace Z_ERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class inv_ItemHistoryProccessTypes
    {
        [Key]
        public int ItemHistoryProccessTypeID { get; set; }

        [StringLength(50)]
        public string ItemHistoryProccessTypeAR { get; set; }

        [StringLength(50)]
        public string ItemHistoryProccessTypeEn { get; set; }
    }
}

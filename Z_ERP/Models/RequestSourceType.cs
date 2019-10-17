namespace Z_ERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RequestSourceType
    {
        public int RequestSourceTypeID { get; set; }

        [StringLength(50)]
        public string RequestSourceTypeName { get; set; }
    }
}

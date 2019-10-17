namespace Z_ERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class inv_Requests
    {
        [Key]
        public int RequestID { get; set; }

        public int? RequestSourceTypeID { get; set; }

        public int? RequestSourceID { get; set; }

        public int? RequestDestinationID { get; set; }

        [StringLength(200)]
        public string RequestSourceName { get; set; }

        [StringLength(200)]
        public string RequestNo { get; set; }

        public decimal? RequestTotalSaleAmount { get; set; }

        public decimal? RequestPaidAmount { get; set; }

        public decimal? RequestRemaining { get; set; }

        public bool? RequestStatus { get; set; }

        [Column(TypeName = "date")]
        public DateTime? RequestDate { get; set; }

        public bool? UpLoaded { get; set; }
    }
}

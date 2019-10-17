namespace Z_ERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class inv_POSRequests
    {
        [Key]
        public int RequestID { get; set; }

        public int? PointOfSaleID { get; set; }

        [StringLength(200)]
        public string PointOfSaleName { get; set; }

        [StringLength(200)]
        public string RequestNo { get; set; }

        public decimal? RequestTotalAmount { get; set; }

        public decimal? RequestPaidAmount { get; set; }

        public decimal? RequestRemaining { get; set; }

        public bool? RequestStatus { get; set; }

        [Column(TypeName = "date")]
        public DateTime? RequestDate { get; set; }
    }
}

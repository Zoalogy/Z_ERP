namespace Z_ERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class trc_Trips
    {
        [Key]
        public int TripID { get; set; }

        [StringLength(200)]
        public string TripName { get; set; }

        public int? TruckID { get; set; }

        [StringLength(200)]
        public string TripDescription { get; set; }

        public bool? UpLoaded { get; set; }

        [Column(TypeName = "date")]
        public DateTime? TripStartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? TripEndDate { get; set; }

        public int? TripStatusID { get; set; }
    }
}

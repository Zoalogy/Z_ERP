namespace Z_ERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class trc_Trucks
    {
        [Key]
        public int TruckID { get; set; }

        [StringLength(200)]
        public string TruckNameAr { get; set; }

        [StringLength(200)]
        public string TruckNameEn { get; set; }

        [StringLength(200)]
        public string TruckNumber { get; set; }

        [StringLength(200)]
        public string DriverName { get; set; }

        [StringLength(200)]
        public string DriverAssistant { get; set; }

        public bool? Status { get; set; }

        public bool? UpLoaded { get; set; }

        [Column(TypeName = "date")]
        public DateTime? TruckAddedDate { get; set; }
    }
}

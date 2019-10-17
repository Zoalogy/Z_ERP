namespace Z_ERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class inv_Stror_to_Store_order
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Stror_to_Store_orderID { get; set; }
        public int? orderItemID { get; set; }

        public int? ordernTOventoryID { get; set; }

        public int? orderFromnventoryID { get; set; }

        [StringLength(200)]
        public string orderItemName { get; set; }

        public int? orderItemQuantity { get; set; }

        public int? orderStaus { get; set; }

        [StringLength(200)]
        public string orderUserName { get; set; }

        [StringLength(200)]
        public string toOrderInventoryName { get; set; }

        [StringLength(200)]
        public string fromOrderInventoryName { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fromOrderDate { get; set; }
        
        [Column(Order = 1)]
        public bool UpLoaded { get; set; }

        [Column(TypeName = "date")]
        public DateTime? toOrdeererDate { get; set; }
    }
}

namespace Z_ERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class inv_RequestCart
    {
        [Key]
        public int RequestCartID { get; set; }

        public int? ItemID { get; set; }

        [StringLength(200)]
        public string ItemName { get; set; }

        public int? ItemQuantity { get; set; }

        public decimal? ItemPrice { get; set; }

        public decimal? TotalItemsPrice { get; set; }
        public int? RequestType { get; set; }



        

        [Column(TypeName = "date")]
        public DateTime? RequestCartDate { get; set; }
    }
}

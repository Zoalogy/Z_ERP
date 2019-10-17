namespace Z_ERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class pur_Supliers
    {
        [Key]
        public int SuplierID { get; set; }

        [StringLength(100)]
        public string SuplierName { get; set; }

        [StringLength(100)]

        public string SuplierPhone { get; set; }

        [StringLength(100)]
        public string SuplierEmail { get; set; }
        public string SuplierBankAcount { get; set; }
        

        [Column(TypeName = "date")]
        public DateTime? SuplierRegisterationDate { get; set; }

        public bool? UpLoaded { get; set; }
    }
}

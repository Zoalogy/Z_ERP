namespace Z_ERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class pay_PaymentMethod
    {
        [Key]
        public int PaymentMethodID { get; set; }

        [StringLength(100)]
        public string PaymentMethod { get; set; }

        public bool? UpLoaded { get; set; }
    }
}

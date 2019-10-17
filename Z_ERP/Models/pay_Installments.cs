namespace Z_ERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class pay_Installments
    {
        [Key]
        public int InstallmentsID { get; set; }

        [StringLength(10)]
        public string RecieptNo { get; set; }

        public int? PaymentMethodID { get; set; }

        public decimal? InstallmentsDepitAmount { get; set; }

        public decimal? InstallmentsCreditAmount { get; set; }

        public bool? InstallmentsIsPaid { get; set; }

        [StringLength(200)]
        public string InstallmentsDescription { get; set; }

        [Column(TypeName = "date")]
        public DateTime? InstallmentsDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? InstallmentsPayedDate { get; set; }

        public bool? UpLoaded { get; set; }
    }
}

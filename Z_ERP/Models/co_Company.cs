namespace Z_ERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class co_Company
    {
        [Key]
        public int CompanyID { get; set; }

        [StringLength(100)]
        public string CompanyNameAr { get; set; }

        [StringLength(100)]
        public string CompanyNameEn { get; set; }

        [StringLength(100)]
        public string CompanyPhone { get; set; }

        [StringLength(100)]
        public string CompanyEmail { get; set; }

        [StringLength(100)]
        public string CompanyAddressAr { get; set; }

        [StringLength(100)]
        public string CompanyAddressEn { get; set; }
    }
}

namespace Z_ERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class hr_Gender
    {
        [Key]
        public int GenderId { get; set; }

        [StringLength(10)]
        public string GenderNmaeAr { get; set; }

        [StringLength(10)]
        public string GenderNmaeEn { get; set; }
    }
}

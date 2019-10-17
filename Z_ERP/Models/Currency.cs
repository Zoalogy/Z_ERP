namespace Z_ERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Currency
    {
        public int CurrencyID { get; set; }

        [StringLength(50)]
        public string CurrencyNameAr { get; set; }

        [StringLength(50)]
        public string CurrencyNameEn { get; set; }

        [StringLength(50)]
        public string CurrencyShortName { get; set; }

        public int? CurrencyExchangeRate { get; set; }

        public bool? CurrencyStatus { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CurrencyDate { get; set; }

        public bool? UpLoaded { get; set; }
    }
}

namespace Z_ERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sys_SystemScreenDialogs
    {
        [Key]
        [Column(Order = 0)]
        public int id { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string ScreenDailogID { get; set; }

        [StringLength(50)]
        public string ScreenNameAr { get; set; }

        [StringLength(50)]
        public string ScreenNameEn { get; set; }

        [StringLength(50)]
        public string FontIcon { get; set; }

        [StringLength(50)]
        public string URL { get; set; }

        [StringLength(500)]
        public string Discription { get; set; }

        public int? SubMenuId { get; set; }

        public bool? IsActive { get; set; }

        public int? MenuIndex { get; set; }

        public int? IsLeaf { get; set; }
    }
}

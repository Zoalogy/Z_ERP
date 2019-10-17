namespace Z_ERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sys_PermissionLabels
    {
        public int ID { get; set; }

        public int? ComponetID { get; set; }

        public int? PermissionID { get; set; }

        [StringLength(100)]
        public string ComponentName { get; set; }

        [StringLength(250)]
        public string ALabel { get; set; }

        [Required]
        [StringLength(250)]
        public string Elabel { get; set; }

        [StringLength(250)]
        public string APlaceholder { get; set; }

        [StringLength(250)]
        public string EPlaceholder { get; set; }

        public bool? isRequired { get; set; }

        [StringLength(250)]
        public string RequiredMessage { get; set; }

        [StringLength(50)]
        public string ComponentTypeId { get; set; }

        [StringLength(50)]
        public string ComponentKeyName { get; set; }

        public int? ComponetIndex { get; set; }

        public int? GroupNo { get; set; }

        public int? LookupTypeData { get; set; }
    }
}

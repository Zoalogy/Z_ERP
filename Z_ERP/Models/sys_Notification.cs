namespace Z_ERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sys_Notification
    {
        public int id { get; set; }

        [StringLength(50)]
        public string NotificationTitle { get; set; }

        [StringLength(50)]
        public string Url { get; set; }

        public int? status { get; set; }

        [StringLength(250)]
        public string NotificationBody { get; set; }

        public int? UserGroupId { get; set; }

        public int? BranchID { get; set; }

        public int? JobID { get; set; }

        public int? ProductGroupID { get; set; }

        public TimeSpan? NotificationTime { get; set; }

        public int? Source { get; set; }
    }
}

namespace Z_ERP.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class TesttModel : DbContext
    {
        public TesttModel()
            : base("name=TesttModel")
        {
        }

        public virtual DbSet<sal_SalesCart> sal_SalesCart { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<sal_SalesCart>()
                .Property(e => e.ItemPrice)
                .HasPrecision(18, 0);

            modelBuilder.Entity<sal_SalesCart>()
                .Property(e => e.TotalItemsPrice)
                .HasPrecision(18, 0);
        }
    }
}

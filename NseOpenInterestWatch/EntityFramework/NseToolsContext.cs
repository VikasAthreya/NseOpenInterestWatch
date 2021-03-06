namespace NseOpenInterestWatch.EntityFramework
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class NseToolsContext : DbContext
    {
        public NseToolsContext()
            : base("name=NseToolsContext")
        {
        }

        public virtual DbSet<BankNiftyOiHistory> BankNiftyOiHistories { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}

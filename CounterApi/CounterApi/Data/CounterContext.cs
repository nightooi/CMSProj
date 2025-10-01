using CounterApi.Data.Model;

using Microsoft.EntityFrameworkCore;

namespace CounterApi.Data
{
    public class CounterContext : DbContext
    {
        public CounterContext(DbContextOptions<CounterContext> options) : base(options) 
        { 

        }
        public DbSet<Counter> Counters { get; set; }
        public DbSet<CounterUpdate> CounterUpdates { get; set; }
        protected override void OnModelCreating(ModelBuilder b)
        {
            b.Entity<Counter>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Page).IsRequired().HasMaxLength(200);
                e.HasIndex(x => x.Page).IsUnique();
                e.Property(x => x.Count).IsRequired();
                e.HasMany(x => x.UpdateLog).WithOne(x => x.Counter).HasForeignKey(x => x.CounterId).OnDelete(DeleteBehavior.Cascade);
            });
            b.Entity<CounterUpdate>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.RequestTime).IsRequired();
                e.Property(x => x.LogMessage).HasMaxLength(400);
            });
        }
    }
}

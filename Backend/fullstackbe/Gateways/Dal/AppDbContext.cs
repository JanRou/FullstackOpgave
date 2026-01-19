using fullstackbe.Gateways.Dal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace fullstackbe.Gateways.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<VirksomhedDao> Virksomheds { get; init; }

        // Måske creme og sukker: Kan SQLite det her og er det nødvendigt?
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VirksomhedDao>()
                .HasKey(x => x.Cvr)
                .HasName("VirksomhedCvr_PK");
        }
    }
}

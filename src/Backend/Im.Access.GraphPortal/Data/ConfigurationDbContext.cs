using Microsoft.EntityFrameworkCore;

namespace Im.Access.GraphPortal.Data
{
    public class ConfigurationDbContext : DbContext
    {
        public ConfigurationDbContext(DbContextOptions<ConfigurationDbContext> options) : base(options)
        {
        }

        public virtual DbSet<DbClient> Clients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<DbClient>()
                .ToTable("AspNetClients", "dbo")
                .HasKey(e => e.Id);
            modelBuilder
                .Entity<DbClient>()
                .HasMany(c => c.Claims)
                .WithOne();

            modelBuilder
                .Entity<DbClientClaim>()
                .ToTable("AspNetClientClaims", "dbo")
                .HasKey(e => e.Id);
        }
    }
}
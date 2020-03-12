using Microsoft.EntityFrameworkCore;

namespace Im.Access.GraphPortal.Data
{
    public class OperationDbContext : DbContext
    {
        public OperationDbContext(DbContextOptions<OperationDbContext> options) : base(options)
        {
        }

        public virtual DbSet<DbChaosPolicy> ChaosPolicies { get; set; }

        public virtual DbSet<DbCircuitBreakerPolicy> CircuitBreakerPolicies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DbChaosPolicy>(
                cp =>
                {
                    cp.HasKey(p => p.Id);
                    cp.Property(p => p.MachineName)
                        .HasMaxLength(50)
                        .IsRequired();
                    cp.Property(p => p.ServiceName)
                        .HasMaxLength(50)
                        .IsRequired();
                    cp.Property(p => p.PolicyKey)
                        .HasMaxLength(128)
                        .IsRequired();
                    cp.Property(p => p.Enabled)
                        .IsRequired();
                    cp.Property(p => p.FaultEnabled)
                        .IsRequired();
                    cp.Property(p => p.FaultInjectionRate)
                        .IsRequired();
                    cp.Property(p => p.LatencyEnabled)
                        .IsRequired();
                    cp.Property(p => p.LatencyInjectionRate)
                        .IsRequired();
                    cp.Property(p => p.LastUpdated)
                        .IsRequired();
                    cp.ToTable("ChaosPolicies");
                });

            modelBuilder.Entity<DbCircuitBreakerPolicy>(
                cp =>
                {
                    cp.HasKey(p => p.Id);
                    cp.Property(p => p.MachineName)
                        .HasMaxLength(50)
                        .IsRequired();
                    cp.Property(p => p.ServiceName)
                        .HasMaxLength(50)
                        .IsRequired();
                    cp.Property(p => p.PolicyKey)
                        .HasMaxLength(128)
                        .IsRequired();
                    cp.Property(p => p.IsIsolated)
                        .IsRequired();
                    cp.Property(p => p.LastUpdated)
                        .IsRequired();
                    cp.ToTable("CircuitBreakerPolicies");
                });
        }
    }
}
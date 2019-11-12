using Microsoft.EntityFrameworkCore;

namespace Im.Access.GraphPortal.Data
{
    public class IdentityDbContext : DbContext
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
        {
        }

        public virtual DbSet<DbUser> Users { get; set; }

        public virtual DbSet<DbUserClaim> UserClaims { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<DbUser>()
                .ToTable("AspNetUsers", "dbo")
                .HasKey(e => e.Id);
            modelBuilder
                .Entity<DbUser>()
                .Property(e => e.Id)
                .HasMaxLength(128)
                .IsRequired();
            modelBuilder
                .Entity<DbUser>()
                .Property(e => e.TenantId)
                .HasMaxLength(128)
                .IsRequired();
            modelBuilder
                .Entity<DbUser>()
                .Property(e => e.FirstName);
            modelBuilder
                .Entity<DbUser>()
                .Property(e => e.LastName);
            modelBuilder
                .Entity<DbUser>()
                .Property(e => e.Email)
                .HasMaxLength(256);
            modelBuilder
                .Entity<DbUser>()
                .Property(e => e.EmailConfirmed)
                .IsRequired();
            modelBuilder
                .Entity<DbUser>()
                .Property(e => e.PasswordHash);
            modelBuilder
                .Entity<DbUser>()
                .Property(e => e.SecurityStamp);
            modelBuilder
                .Entity<DbUser>()
                .Property(e => e.PhoneNumber);
            modelBuilder
                .Entity<DbUser>()
                .Property(e => e.PhoneNumberConfirmed)
                .IsRequired();
            modelBuilder
                .Entity<DbUser>()
                .Property(e => e.TwoFactorEnabled)
                .IsRequired();
            modelBuilder
                .Entity<DbUser>()
                .Property(e => e.LockoutEndDateUtc);
            modelBuilder
                .Entity<DbUser>()
                .Property(e => e.LockoutEnabled);
            modelBuilder
                .Entity<DbUser>()
                .Property(e => e.AccessFailedCount)
                .IsRequired();
            modelBuilder
                .Entity<DbUser>()
                .Property(e => e.UserName)
                .HasMaxLength(256)
                .IsRequired();
            modelBuilder
                .Entity<DbUser>()
                .Property(e => e.RegistrationDate);
            modelBuilder
                .Entity<DbUser>()
                .Property(e => e.LastLoggedInDate);
            modelBuilder
                .Entity<DbUser>()
                .Property(e => e.RegistrationIPAddress)
                .HasMaxLength(50);
            modelBuilder
                .Entity<DbUser>()
                .Property(e => e.LastLoggedInIPAddress)
                .HasMaxLength(50);
            modelBuilder
                .Entity<DbUser>()
                .Property(e => e.ScreenName)
                .HasMaxLength(100)
                .IsRequired();
            modelBuilder
                .Entity<DbUser>()
                .Property(e => e.LastUpdatedDate)
                .IsRequired();
            modelBuilder
                .Entity<DbUser>()
                .Property(e => e.CreateDate)
                .IsRequired();
            modelBuilder
                .Entity<DbUser>()
                .Property(e => e.UserType)
                .HasMaxLength(50);
            modelBuilder
                .Entity<DbUser>()
                .Property(e => e.Address1)
                .HasMaxLength(255);
            modelBuilder
                .Entity<DbUser>()
                .Property(e => e.Address2)
                .HasMaxLength(255);
            modelBuilder
                .Entity<DbUser>()
                .Property(e => e.City)
                .HasMaxLength(50);
            modelBuilder
                .Entity<DbUser>()
                .Property(e => e.Country)
                .HasMaxLength(100);
            modelBuilder
                .Entity<DbUser>()
                .Property(e => e.Postcode)
                .HasMaxLength(20);
            modelBuilder
                .Entity<DbUser>()
                .Property(e => e.UserBiography)
                .HasMaxLength(4000);
            modelBuilder
                .Entity<DbUser>()
                .Property(e => e.FirstPartyIM)
                .IsRequired();
            modelBuilder
                .Entity<DbUser>()
                .Property(e => e.County)
                .HasMaxLength(50);
            modelBuilder
                .Entity<DbUser>()
                .Property(e => e.FirstPartyImUpdatedDate)
                .IsRequired();
            modelBuilder
                .Entity<DbUser>()
                .Property(e => e.AuthenticationType)
                .HasMaxLength(20);
            modelBuilder
                .Entity<DbUser>()
                .HasMany(e => e.Claims)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder
                .Entity<DbUserClaim>()
                .ToTable("AspNetUserClaims", "dbo")
                .HasKey(e => e.Id)
                .IsClustered();
            modelBuilder
                .Entity<DbUserClaim>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();
            modelBuilder
                .Entity<DbUserClaim>()
                .Property(e => e.UserId)
                .IsRequired();
            modelBuilder
                .Entity<DbUserClaim>()
                .Property(e => e.ClaimType);
            modelBuilder
                .Entity<DbUserClaim>()
                .Property(e => e.ClaimValue);
            modelBuilder
                .Entity<DbUserClaim>()
                .Property(e => e.ClaimUpdatedDate)
                .IsRequired();
        }
    }
}
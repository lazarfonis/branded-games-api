using BrandedGames.Entities;
using BrandedGames.Entities.Identity;
using BrandedGames.Entities.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrandedGames.Data;

public class BrandedGamesDbContext : IdentityDbContext<User, Role, Guid, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
{
    public virtual DbSet<GameForm> GameForms { get; set; }
    public virtual DbSet<GameFeature> GameFeatures { get; set; }
    public virtual DbSet<GameFormFeature> GameFormFeatures { get; set; }
    public virtual DbSet<GameFormFile> GameFormFiles { get; set; }
    public virtual DbSet<GameFormPlatformType> GameFormPlatformTypes { get; set; }
    public virtual DbSet<GameType> GameTypes { get; set; }
    public virtual DbSet<PlatformType> PlatformTypes { get; set; }

    public BrandedGamesDbContext(DbContextOptions<BrandedGamesDbContext> options) : base(options)
    {
        // Migrations only apply to relational providers. Guarded so non-relational providers
        // (for example the EF Core in-memory provider used by the test suite) can construct
        // the context without attempting to run relational migrations.
        if (Database.IsRelational())
        {
            Database.Migrate();
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<GameFormFeature>(entity =>
        {
            entity.HasKey(gff => new { gff.GameFormId, gff.GameFeatureId });
        });

        modelBuilder.Entity<GameFormPlatformType>(entity =>
        {
            entity.HasKey(gff => new { gff.GameFormId, gff.PlatformTypeId });
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasMany(u => u.Claims)
                .WithOne(uc => uc.User)
                .HasForeignKey(uc => uc.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            entity.HasMany(u => u.Logins)
                .WithOne(ul => ul.User)
                .HasForeignKey(ul => ul.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            entity.HasMany(u => u.Tokens)
                .WithOne(ut => ut.User)
                .HasForeignKey(ut => ut.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            entity.HasMany(u => u.UserRoles)
                .WithOne(ur => ur.User)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            entity.HasIndex(u => u.Email).IsUnique();
        });
    }

    public override int SaveChanges()
    {
        PopulateEntityFields();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        PopulateEntityFields();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private void PopulateEntityFields()
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is IEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entityEntry in entries)
        {
            if (entityEntry.State == EntityState.Added)
            {
                var entity = (IEntity)entityEntry.Entity;
                if (entity.CreatedAt == default)
                {
                    entity.CreatedAt = DateTime.UtcNow;
                }
            }
            else if (entityEntry.State == EntityState.Modified)
            {
                ((IEntity)entityEntry.Entity).ModifiedAt = DateTime.UtcNow;
            }
        }
    }
}

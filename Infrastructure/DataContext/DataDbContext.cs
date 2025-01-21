using Domain.Entities.UserEntity;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.Inventory;

namespace Infrastructure.DataContext;

public class DataDbContext(DbContextOptions<DataDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<UnitOfMeasure> UnitOfMeasures { get; set; }
    public DbSet<UnitConversion> UnitConversions { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Item> Items { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasIndex(x => x.Email).IsUnique();
        modelBuilder.Entity<User>().HasIndex(x => x.Username).IsUnique();
        modelBuilder.Entity<UnitOfMeasure>().HasIndex(x => x.UnitName).IsUnique();
        modelBuilder.Entity<Item>().HasIndex(x => x.Name).IsUnique();
        modelBuilder.Entity<Category>().HasIndex(x => x.Name).IsUnique();
        modelBuilder.Entity<UnitConversion>()
            .HasOne(u => u.FromUnit)
            .WithMany()
            .HasForeignKey(u => u.FromUnitId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UnitConversion>()
            .HasOne(u => u.ToUnit)
            .WithMany()
            .HasForeignKey(u => u.ToUnitId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Category>()
            .HasOne(c => c.Parent)
            .WithMany()
            .HasForeignKey(c => c.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Item>()
       .HasOne(i => i.Category)
       .WithMany(c => c.Items)
       .HasForeignKey(i => i.CategoryId)
       .OnDelete(DeleteBehavior.Restrict);

    }
}
using Domain.Entities.UserEntity;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.Inventory;

namespace Infrastructure.DataContext;

public class DataDbContext(DbContextOptions<DataDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<UnitOfMeasure> UnitOfMeasures { get; set; }
    public DbSet<UnitConversion> UnitConversions { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Inventory> Inventories { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasIndex(x=>x.Email).IsUnique();
        modelBuilder.Entity<User>().HasIndex(x=>x.Username).IsUnique();
        modelBuilder.Entity<UnitOfMeasure>().HasIndex(x=>x.UnitName).IsUnique();
        modelBuilder.Entity<Product>().HasIndex(x=>x.ProductName).IsUnique();
        modelBuilder.Entity<Category>().HasIndex(x=>x.CategoryName).IsUnique();
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
    }
}
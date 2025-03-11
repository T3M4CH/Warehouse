using Microsoft.EntityFrameworkCore;
using Warehouse.Entities;

namespace Warehouse.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>().ToTable("Products");

        modelBuilder.Entity<Animal>().ToTable("Animals");
        modelBuilder.Entity<Food>().ToTable("Food");
        modelBuilder.Entity<Clothes>().ToTable("Clothes");

        modelBuilder.Entity<Animal>().HasBaseType<Product>();
        modelBuilder.Entity<Food>().HasBaseType<Product>();
        modelBuilder.Entity<Clothes>().HasBaseType<Product>();
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Animal> Animals { get; set; }
    public DbSet<Clothes> Clothes { get; set; }
    public DbSet<Food> Foods { get; set; }
    public DbSet<Container> Containers { get; set; }
    public DbSet<Warehouses> Warehouses { get; set; }
}
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Warehouse.Entities;
using WarehouseApi.Entities;

namespace Warehouse.Data;

public class DataContext : IdentityDbContext<UserEntity>
{
    public DataContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ProductEntity>().ToTable("Products");

        modelBuilder.Entity<AnimalEntity>().ToTable("Animals");
        modelBuilder.Entity<FoodEntity>().ToTable("Food");
        modelBuilder.Entity<ClothesEntity>().ToTable("Clothes");

        modelBuilder.Entity<AnimalEntity>().HasBaseType<ProductEntity>();
        modelBuilder.Entity<FoodEntity>().HasBaseType<ProductEntity>();
        modelBuilder.Entity<ClothesEntity>().HasBaseType<ProductEntity>();

        modelBuilder.Entity<UserWarehouseEntity>()
            .HasKey(uw => new { uw.UserId, uw.WarehouseId });

        modelBuilder.Entity<UserWarehouseEntity>()
            .HasOne(uw => uw.WarehouseEntity)
            .WithMany(w => w.UserWarehouses)
            .HasForeignKey(wc => wc.WarehouseId);

        modelBuilder.Entity<UserWarehouseEntity>()
            .HasOne(uw => uw.User)
            .WithMany(w => w.UserWarehouses)
            .HasForeignKey(wc => wc.UserId);
    }

    public DbSet<ProductEntity> Products { get; set; }
    public DbSet<AnimalEntity> Animals { get; set; }
    public DbSet<ClothesEntity> Clothes { get; set; }
    public DbSet<FoodEntity> Foods { get; set; }
    public DbSet<ContainerEntity> Containers { get; set; }
    public DbSet<WarehouseEntity> Warehouses { get; set; }
    public DbSet<UserWarehouseEntity> UserWarehouses { get; set; }
}
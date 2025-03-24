using Microsoft.EntityFrameworkCore;
using Warehouse.Data;
using Warehouse.Entities;
using Warehouse.Helpers;
using WarehouseApi.Repositories.Interfaces;

namespace WarehouseApi.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly DataContext _dataContext;

    public ProductRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<bool> AnyAsync()
    {
        return await _dataContext.Products.AnyAsync();
    }

    public async Task AddProduct(ProductEntity productEntity)
    {
        await _dataContext.Products.AddAsync(productEntity);
    }

    public async Task<IEnumerable<ProductEntity>> GetProductsAsync()
    {
        return await _dataContext.Products.ToListAsync();
    }

    public async Task<ProductEntity?> GetProductByIdAsync(int id)
    {
        return await _dataContext.Products.FindAsync(id);
    }

    public void RemoveProduct(ProductEntity productEntity)
    {
        _dataContext.Products.Remove(productEntity);
    }

    public async Task AddProducts(IEnumerable<ProductEntity> products)
    {
        products = products.ToList();
        var animals = products.OfType<AnimalEntity>().ToList();
        var foods = products.OfType<FoodEntity>().ToList();
        var clothes = products.OfType<ClothesEntity>().ToList();

        if (animals.Any())
        {
            await _dataContext.Animals.AddRangeAsync(animals);
        }

        if (foods.Any())
        {
            await _dataContext.Foods.AddRangeAsync(foods);
        }

        if (clothes.Any())
        {
            await _dataContext.Clothes.AddRangeAsync(clothes);
        }
    }

    public async Task<List<ProductEntity>> GetProductsListByIdsAsync(ICollection<int> dtoProductIds)
    {
        return await _dataContext.Products.Where(p => dtoProductIds.Contains(p.Id)).ToListAsync();
    }
}
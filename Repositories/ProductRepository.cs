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

    public async Task AddProduct(Product product)
    {
        await _dataContext.Products.AddAsync(product);
    }

    public async Task<IEnumerable<Product>> GetProductsAsync()
    {
        return await _dataContext.Products.ToListAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await _dataContext.Products.FindAsync(id);
    }

    public void RemoveProduct(Product product)
    {
        _dataContext.Products.Remove(product);
    }

    public async Task AddProducts(IEnumerable<Product> products)
    {
        products = products.ToList();
        var animals = products.OfType<Animal>().ToList();
        var foods = products.OfType<Food>().ToList();
        var clothes = products.OfType<Clothes>().ToList();

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

    public async Task<List<Product>> GetProductsListByIdsAsync(ICollection<int> dtoProductIds)
    {
        return await _dataContext.Products.Where(p => dtoProductIds.Contains(p.Id)).ToListAsync();
    }
}
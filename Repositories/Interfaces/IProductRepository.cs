using Warehouse.Entities;
using Warehouse.Helpers;

namespace WarehouseApi.Repositories.Interfaces;

public interface IProductRepository
{
    Task<bool> AnyAsync();
    Task AddProduct(Product product);
    Task<IEnumerable<Product>> GetProductsAsync();
    Task<Product?> GetProductByIdAsync(int id);
    void RemoveProduct(Product product);
    Task AddProducts(IEnumerable<Product> products);
    Task<List<Product>> GetProductsListByIdsAsync(ICollection<int> dtoProductIds);
}
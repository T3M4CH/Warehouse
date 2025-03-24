using Warehouse.Entities;
using Warehouse.Helpers;

namespace WarehouseApi.Repositories.Interfaces;

public interface IProductRepository
{
    Task<bool> AnyAsync();
    Task AddProduct(ProductEntity productEntity);
    Task<IEnumerable<ProductEntity>> GetProductsAsync();
    Task<ProductEntity?> GetProductByIdAsync(int id);
    void RemoveProduct(ProductEntity productEntity);
    Task AddProducts(IEnumerable<ProductEntity> products);
    Task<List<ProductEntity>> GetProductsListByIdsAsync(ICollection<int> dtoProductIds);
}
using Warehouse.Entities;

namespace WarehouseApi.Repositories.Interfaces;

public interface IContainerRepository
{
    Task<Container?> GetByIdAsync(int id);
    Task<IEnumerable<Container>> GetByWarehouseIdAsync(int warehouseId);
    Task<List<Container>> GetContainersWithProductsAsync();
    Task AddAsync(Container container);
    Task RemoveAsync(Container container);
}
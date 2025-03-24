using Warehouse.Entities;

namespace WarehouseApi.Repositories.Interfaces;

public interface IContainerRepository
{
    Task<ContainerEntity?> GetByIdAsync(int id);
    Task<IEnumerable<ContainerEntity>> GetByWarehouseIdAsync(int warehouseId);
    Task<List<ContainerEntity>> GetContainersWithProductsAsync();
    Task AddAsync(ContainerEntity containerEntity);
    Task RemoveAsync(ContainerEntity containerEntity);
}
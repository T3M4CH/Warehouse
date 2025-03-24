using WarehouseApi.Entities;

namespace WarehouseApi.Repositories.Interfaces;

public interface IWarehouseRepository
{
    Task<bool> BelongsToUser(string userId, int warehouseId);
    Task<WarehouseEntity> CreateWarehouse(WarehouseEntity warehouse, string userId);
    Task AddAsync(WarehouseEntity warehouseEntity);
    Task<bool> ExistByIdAsync(int id);
    Task<WarehouseEntity?> GetByIdAsync(int id);
    Task<IEnumerable<WarehouseEntity>> GetAllWithContainersAsync();
    Task<double> GetTotalWeightAsync(int warehouseId);
}
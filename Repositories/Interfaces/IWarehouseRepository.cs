using Microsoft.AspNetCore.Mvc;
using WarehouseApi.Entities;

namespace WarehouseApi.Repositories.Interfaces;

public interface IWarehouseRepository
{
    Task AddAsync(WarehouseEntity warehouseEntity);
    Task<bool> ExistByIdAsync(int id);
    Task<WarehouseEntity?> GetByIdAsync(int id);
    Task<IEnumerable<WarehouseEntity>> GetAllWithContainersAsync();
    Task<double> GetTotalWeightAsync(int warehouseId);
}
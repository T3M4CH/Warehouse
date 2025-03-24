using Warehouse.DTOs;
using Warehouse.Helpers;
using WarehouseApi.Entities;

namespace Warehouse.Services.Interfaces;

public interface IWarehouseService
{
    Task<OperationResult<WarehouseEntity>> CreateWarehouse(CreateWarehouseDto dto, string userId);
    Task<OperationResult> GetWarehouses();
    Task<OperationResult> AddContainer(AddContainerDto dto);
    Task<OperationResult> SendContainer(SendContainerDto dto, string userId);
    Task<OperationResult<double>> GetTotalWeight(int warehouseId);
    Task<OperationResult> GetContainers(int warehouseId);
}
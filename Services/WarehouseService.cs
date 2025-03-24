using Newtonsoft.Json;
using Warehouse.DTOs;
using Warehouse.Helpers;
using Warehouse.Services.Interfaces;
using Warehouse.UnitOfWork.Interfaces;
using WarehouseApi.Entities;

namespace Warehouse.Services;

public class WarehouseService : IWarehouseService
{
    private readonly IUnitOfWork _unitOfWork;

    public WarehouseService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<OperationResult<WarehouseEntity>> CreateWarehouse(CreateWarehouseDto dto, string userId)
    {
        var warehouse = new WarehouseEntity()
        {
            Location = dto.Location,
        };

        await _unitOfWork.WarehouseRepository.CreateWarehouse(warehouse, userId);

        await _unitOfWork.CommitAsync();

        return OperationResult<WarehouseEntity>.Success(warehouse);
    }

    public async Task<OperationResult> GetWarehouses()
    {
        var warehouses = await _unitOfWork.WarehouseRepository.GetAllWithContainersAsync();
        var warehousesInfo = warehouses.Select(w => new
        {
            w.Id,
            w.Location,
            ContainersCount = w.Containers.Count
        });

        var info = JsonConvert.SerializeObject(warehousesInfo);

        return OperationResult.Success(info);
    }

    public async Task<OperationResult> AddContainer(AddContainerDto dto)
    {
        var warehouse = await _unitOfWork.WarehouseRepository.GetByIdAsync(dto.WarehouseId);
        if (warehouse == null)
            return OperationResult.Failure($"WarehouseApi with ID {dto.WarehouseId} not found.");

        var container = await _unitOfWork.ContainerRepository.GetByIdAsync(dto.ContainerId);
        if (container == null)
            return OperationResult.Failure($"Container with ID {dto.ContainerId} not found.");

        if (container.WarehouseId.HasValue)
            return OperationResult.Failure(container.WarehouseId.Value == warehouse.Id ? "Container already in that warehouse" : "Container on the other warehouse use SendContainer");

        container.WarehouseId = dto.WarehouseId;
        warehouse.Containers.Add(container);

        await _unitOfWork.CommitAsync();

        return OperationResult.Success($"Container {container.Id} was added to warehouse {warehouse.Id}.");
    }

    public async Task<OperationResult> SendContainer(SendContainerDto dto, string userId)
    {
        var isUserWarehouse = await _unitOfWork.WarehouseRepository.BelongsToUser(userId, dto.ContainerId);
        if (!isUserWarehouse)
            return OperationResult.Failure("Forbid");

        var container = await _unitOfWork.ContainerRepository.GetByIdAsync(dto.ContainerId);
        if (container == null || container.WarehouseId != dto.SenderWarehouseId)
            return OperationResult.Failure("Container does not belong to the sender warehouse.");

        var targetWarehouse = await _unitOfWork.WarehouseRepository.GetByIdAsync(dto.TargetWarehouseId);
        if (targetWarehouse == null)
            return OperationResult.Failure($"Target warehouse {dto.TargetWarehouseId} not found.");

        container.WarehouseId = dto.TargetWarehouseId;
        targetWarehouse.Containers.Add(container);

        await _unitOfWork.CommitAsync();

        return OperationResult.Success();
    }

    public async Task<OperationResult<double>> GetTotalWeight(int warehouseId)
    {
        var totalWeight = await _unitOfWork.WarehouseRepository.GetTotalWeightAsync(warehouseId);

        return OperationResult<double>.Success(totalWeight);
    }

    public async Task<OperationResult> GetContainers(int warehouseId)
    {
        var containers = await _unitOfWork.ContainerRepository.GetByWarehouseIdAsync(warehouseId);
        var info = JsonConvert.SerializeObject(containers.Select(c => new
        {
            c.Id,
            c.MaxWeight,
            c.Type,
            c.Category,
            c.WarehouseId
        }));
        
        return OperationResult.Success(info);
    }
}
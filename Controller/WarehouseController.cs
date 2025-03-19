using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Warehouse.UnitOfWork.Interfaces;
using Warehouse.Controller;
using Warehouse.DTOs;
using WarehouseApi.Entities;

[Route("v1/warehouses")]
public class WarehouseApiController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    
    public WarehouseApiController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    [Authorize, HttpPost]
    public async Task<IActionResult> CreateWarehouse(CreateWarehouseDto dto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Unauthorized();
    
        var warehouse = new WarehouseEntity()
        {
            Location = dto.Location,
            UserId = userId
        };
    
        await _unitOfWork.WarehouseRepository.AddAsync(warehouse);
        await _unitOfWork.CommitAsync();
    
        return Ok();
    }
    
    [HttpGet, AllowAnonymous]
    public async Task<IActionResult> GetWarehouses()
    {
        var warehouses = await _unitOfWork.WarehouseRepository.GetAllWithContainersAsync();
        var warehousesInfo = warehouses.Select(w => new
        {
            w.Id,
            w.Location,
            ContainersCount = w.Containers.Count
        });
        return Ok(warehousesInfo);
    }
    
    [Authorize, HttpPost("container")]
    public async Task<IActionResult> AddContainer(AddContainerDto dto)
    {
        var warehouse = await _unitOfWork.WarehouseRepository.GetByIdAsync(dto.WarehouseId);
        if (warehouse == null)
            return NotFound($"WarehouseApi with ID {dto.WarehouseId} not found.");
    
        var container = await _unitOfWork.ContainerRepository.GetByIdAsync(dto.ContainerId);
        if (container == null)
            return NotFound($"Container with ID {dto.ContainerId} not found.");
    
        if (container.WarehouseId.HasValue)
            return BadRequest(container.WarehouseId.Value == warehouse.Id ? "Container already in that warehouse" : "Container on the other warehouse use SendContainer");
    
        container.WarehouseId = dto.WarehouseId;
        warehouse.Containers.Add(container);
    
        await _unitOfWork.CommitAsync();
        return Ok($"Container {container.Id} was added to warehouse {warehouse.Id}.");
    }
    
    [HttpPost("send-container"), Authorize]
    public async Task<IActionResult> SendContainer(SendContainerDto dto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Unauthorized();
    
        var warehouse = await _unitOfWork.WarehouseRepository.GetByIdAsync(dto.SenderWarehouseId);
        if (warehouse == null || warehouse.UserId != userId)
            return Forbid();
    
        var container = await _unitOfWork.ContainerRepository.GetByIdAsync(dto.ContainerId);
        if (container == null || container.WarehouseId != dto.SenderWarehouseId)
            return BadRequest("Container does not belong to the sender warehouse.");
    
        var targetWarehouse = await _unitOfWork.WarehouseRepository.GetByIdAsync(dto.TargetWarehouseId);
        if (targetWarehouse == null)
            return NotFound($"Target warehouse {dto.TargetWarehouseId} not found.");
    
        container.WarehouseId = dto.TargetWarehouseId;
        targetWarehouse.Containers.Add(container);
    
        await _unitOfWork.CommitAsync();
        return Ok($"Container {container.Id} was sent to warehouse {targetWarehouse.Id}.");
    }
    
    [HttpGet("{warehouseId}/total-weight"), AllowAnonymous]
    public async Task<IActionResult> GetTotalWeight(int warehouseId)
    {
        double totalWeight = await _unitOfWork.WarehouseRepository.GetTotalWeightAsync(warehouseId);
        return Ok(new { WarehouseId = warehouseId, TotalWeight = totalWeight });
    }
    
    [HttpGet("{warehouseId}/containers"), AllowAnonymous]
    public async Task<IActionResult> GetContainers(int warehouseId)
    {
        var containers = await _unitOfWork.ContainerRepository.GetByWarehouseIdAsync(warehouseId);
        return Ok(containers.Select(c => new
        {
            c.Id,
            c.MaxWeight,
            c.Type,
            c.Category,
            c.WarehouseId
        }));
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Dynamic.Core;
using Warehouse.Data;
using Warehouse.DTOs;
using Warehouse.Entities;

namespace Warehouse.Controller;

public class WarehouseApiController : BaseApiController
{
    private readonly DataContext _context;

    public WarehouseApiController(DataContext context)
    {
        _context = context;
    }

    [HttpPost("create-warehouse")]
    public async Task<IActionResult> CreateWarehouse(CreateWarehouseDto dto)
    {
        var warehouse = new Warehouses()
        {
            Location = dto.Location
        };

        _context.Add(warehouse);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpGet("get-warehouses")]
    public async Task<IActionResult> GetWarehouses()
    {
        var warehouse = await _context.Warehouses
            .Include(w => w.Containers).ToListAsync();

        var warehousesInfo = warehouse.Select(c => new
        {
            c.Id,
            c.Location,
            c.Containers.Count,
        });

        return Ok(warehousesInfo);
    }

    //TODO: Update/Delete

    [HttpPost("add-container")]
    public async Task<IActionResult> AddContainer(AddContainerDto dto)
    {
        var warehouse = await _context.Warehouses.FindAsync(dto.WarehouseId);
        if (warehouse == null)
            return NotFound($"Warehouse with ID {dto.WarehouseId} not found.");

        var container = await _context.Containers.FindAsync(dto.ContainerId);
        if (container == null)
            return NotFound($"Container with ID {dto.ContainerId} not found.");

        if (container.WarehouseId.HasValue)
            return BadRequest(container.WarehouseId.Value == warehouse.Id ? "Container already in that warehouse" : "Container on the other warehouse use SendContainer");

        container.WarehouseId = dto.WarehouseId;
        warehouse.Containers.Add(container);
        await _context.SaveChangesAsync();

        return Ok($"Container {container.Id} was added to warehouse {warehouse.Id}.");
    }

    [HttpPost("send-container")]
    public async Task<IActionResult> SendContainer(SendContainerDto dto)
    {
        var container = await _context.Containers.FindAsync(dto.ContainerId);
        if (container == null)
            return NotFound($"Container with ID {dto.ContainerId} not found.");

        if (!container.WarehouseId.HasValue || container.WarehouseId.Value != dto.SenderWarehouseId)
            return BadRequest("Container not belongs to that warehouse");

        var currentWarehouse = await _context.Warehouses.FindAsync(container.WarehouseId);
        var targetWarehouse = await _context.Warehouses.FindAsync(dto.TargetWarehouseId);

        if (targetWarehouse == null)
            return NotFound($"Target warehouse {dto.TargetWarehouseId} not found.");

        container.WarehouseId = dto.TargetWarehouseId;
        targetWarehouse.Containers.Add(container);
        await _context.SaveChangesAsync();

        return Ok($"Container {container.Id} was send to warehouse {targetWarehouse.Id}.");
    }

    [HttpGet("{warehouseId}/total-weight")]
    public async Task<IActionResult> GetTotalWeight(int warehouseId)
    {
        var warehouse = await _context.Warehouses
            .Include(w => w.Containers)
            .ThenInclude(c => c.Products)
            .FirstOrDefaultAsync(w => w.Id == warehouseId);

        if (warehouse == null)
            return NotFound($"Warehouse with ID {warehouseId} not found.");

        double totalWeight = warehouse.Containers
            .SelectMany(c => c.Products)
            .Sum(p => p.Weight);

        return Ok(new { WarehouseId = warehouseId, TotalWeight = totalWeight });
    }

    [HttpGet("{warehouseId}/containers")]
    public async Task<IActionResult> GetContainers(int warehouseId)
    {
        var warehouse = await _context.Warehouses
            .Include(w => w.Containers)
            .FirstOrDefaultAsync(w => w.Id == warehouseId);

        if (warehouse == null)
            return NotFound($"Warehouse with ID {warehouseId} not found.");

        var containers = warehouse.Containers.Select(c => new
        {
            c.Id,
            c.MaxWeight,
            c.Type,
            c.Category,
            c.WarehouseId
        });

        return Ok(containers);
    }

    [HttpGet("{warehouseId}/filter-products")]
    public async Task<IActionResult> FilterProducts(int warehouseId, [FromQuery] string? filter)
    {
        var warehouse = await _context.Warehouses
            .Include(w => w.Containers)
            .ThenInclude(c => c.Products)
            .FirstOrDefaultAsync(w => w.Id == warehouseId);

        if (warehouse == null)
            return NotFound($"Warehouse with ID {warehouseId} not found.");

        var filteredProducts = warehouse.Containers
            .SelectMany(c => c.Products)
            .AsQueryable();

        if (!string.IsNullOrEmpty(filter))
        {
            try
            {
                filteredProducts = filteredProducts.Where(filter);
            }
            catch (Exception ex)
            {
                return BadRequest($"Invalid filter syntax: {ex.Message}");
            }
        }

        return Ok(filteredProducts.Select(p => new
        {
            p.Id,
            p.Name,
            p.Weight,
            Type = p.GetType().Name,
            p.ContainerId
        }));
    }
}
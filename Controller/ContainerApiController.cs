using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Warehouse.Containers;
using Warehouse.Data;
using Warehouse.DTOs;
using Warehouse.Entities;
using Warehouse.Enums;
using Warehouse.Helpers;

namespace Warehouse.Controller;

public class ContainerApiController : BaseApiController
{
    private readonly DataContext _context;

    public ContainerApiController(DataContext context)
    {
        _context = context;
    }

    [HttpPost("create-container")]
    public async Task<ActionResult> CreateContainer(CreateContainerDto containerDto)
    {
        if (containerDto.WarehouseId.HasValue)
        {
            var warehouseExists = await _context.Warehouses.AnyAsync(w => w.Id == containerDto.WarehouseId.Value);
            if (!warehouseExists)
            {
                return BadRequest($"Warehouse with ID {containerDto.WarehouseId.Value} does not exist.");
            }
        }

        var container = new Container()
        {
            MaxWeight = containerDto.MaxWeight,
            Type = containerDto.Type,
            Category = containerDto.Category,
            WarehouseId = containerDto.WarehouseId
        };

        _context.Containers.Add(container);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetContainerById), new { id = container.Id }, container);
    }

    [HttpGet("getcontainerbyid")]
    public async Task<ActionResult> GetContainerById(int id)
    {
        var container = await _context.Containers.Include(c => c.Products).FirstOrDefaultAsync(c => c.Id == id);
        if (container == null) return NotFound($"Container with ID {id} is not found");
        return Ok(container);
    }

    [HttpGet("get-all-containers")]
    public async Task<ActionResult> GetContainers()
    {
        var containers = await _context.Containers.Include(c => c.Products).ToListAsync();
        return Ok(containers);
    }

    [HttpPost("add-products")]
    public async Task<IActionResult> AddProductsToContainer(AddProductsToContainerDto dto)
    {
        var container = await _context.Containers
            .FirstOrDefaultAsync(c => c.Id == dto.ContainerId);

        if (container == null)
            return NotFound($"Container with ID {dto.ContainerId} is not found");

        var products = await _context.Products
            .Where(p => dto.ProductIds.Contains(p.Id))
            .ToListAsync();

        var missingProducts = dto.ProductIds.Except(products.Select(p => p.Id)).ToList();
        if (missingProducts.Any())
            return BadRequest($"Products with ID {string.Join(", ", missingProducts)} not found.");

        var differentProducts = products
            .Where(p => dto.ProductIds.Contains(p.Id) && p.Category != container.Category).ToList();
        if (differentProducts.Any())
            return BadRequest($"{string.Join(", ", differentProducts.Select(x => $"{x.Name} - {x.Category}"))} is not the same category as the container {container.Category}.");

        var productsFromOtherContainers = products
            .Where(p => p.ContainerId != container.Id)
            .ToList();

        if (productsFromOtherContainers.Any())
        {
            return BadRequest($"{string.Join(", ", productsFromOtherContainers)} placed on the other warehouse");
        }

        var containerBase = container.Type switch
        {
            EContainerType.Box => new Box(container.MaxWeight),
            EContainerType.Pallet => new Pallet(container.MaxWeight, 25.0),
            _ => throw new ArgumentOutOfRangeException()
        };

        products.AddRange(container.Products);
        try
        {
            containerBase.AddProduct(products);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error with adding product " + ex.Message);
        }

        container.Products.AddRange(products);
        await _context.SaveChangesAsync();

        return Ok($"Products {string.Join(", ", products.Select(p => p.Id))} were added to container {container.Id}.");
    }
}
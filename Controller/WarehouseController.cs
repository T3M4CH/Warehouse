using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Warehouse.Controller;
using Warehouse.DTOs;
using Warehouse.Services.Interfaces;

[Route("v1/warehouses")]
public class WarehouseApiController : BaseApiController
{
    private readonly IWarehouseService _warehouseService;

    public WarehouseApiController(IWarehouseService warehouseService)
    {
        _warehouseService = warehouseService;
    }

    [Authorize, HttpPost]
    public async Task<IActionResult> CreateWarehouse(CreateWarehouseDto dto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Unauthorized();

        var result = await _warehouseService.CreateWarehouse(dto, userId);

        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }

        return BadRequest(result.ErrorMessage);
    }

    [HttpGet, AllowAnonymous]
    public async Task<IActionResult> GetWarehouses()
    {
        var result = await _warehouseService.GetWarehouses();

        if (result.IsSuccess)
        {
            return Ok(result.ExtraMessage);
        }

        return BadRequest(result.ErrorMessage);
    }

    [Authorize, HttpPost("container")]
    public async Task<IActionResult> AddContainer(AddContainerDto dto)
    {
        var result = await _warehouseService.AddContainer(dto);

        if (result.IsSuccess)
        {
            return Ok(result.ExtraMessage);
        }

        return BadRequest(result.ErrorMessage);
    }

    [HttpPost("send-container"), Authorize]
    public async Task<IActionResult> SendContainer(SendContainerDto dto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Unauthorized();

        var result = await _warehouseService.SendContainer(dto, userId);

        if (result.IsSuccess)
        {
            return Ok();
        }

        return BadRequest(result.ErrorMessage);
    }

    [HttpGet("{warehouseId}/total-weight"), AllowAnonymous]
    public async Task<IActionResult> GetTotalWeight(int warehouseId)
    {
        var result = await _warehouseService.GetTotalWeight(warehouseId);

        if (result.IsSuccess)
        {
            return Ok(new { WarehouseId = warehouseId, TotalWeight = result.Data });
        }

        return BadRequest(result.ErrorMessage);
    }

    [HttpGet("{warehouseId}/containers"), AllowAnonymous]
    public async Task<IActionResult> GetContainers(int warehouseId)
    {
        var result = await _warehouseService.GetContainers(warehouseId);

        if (result.IsSuccess)
        {
            return Ok(result.ExtraMessage);
        }

        return BadRequest(result.ErrorMessage);
    }
}
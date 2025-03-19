using Microsoft.AspNetCore.Authorization;
using Warehouse.Containers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Warehouse.Entities;
using Warehouse.Data;
using Warehouse.DTOs;
using Warehouse.Enums;
using Warehouse.Services.Interfaces;
using Warehouse.UnitOfWork.Interfaces;

namespace Warehouse.Controller;

[Route("v1/containers"), AllowAnonymous]
public class ContainerApiController : BaseApiController
{
    private readonly IContainerService _containerService;

    public ContainerApiController(IContainerService containerService)
    {
        _containerService = containerService;
    }

    [HttpPost]
    public async Task<ActionResult> CreateContainer(CreateContainerDto containerDto)
    {
        var result = await _containerService.CreateContainerAsync(containerDto);

        if (result.IsSuccess)
        {
            return CreatedAtAction(nameof(GetContainerById), new { id = result.Data.Id }, result.Data);
        }

        return BadRequest(result.ErrorMessage);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteContainer(int id)
    {
        var result = await _containerService.DeleteContainerAsync(id);

        if (result.IsSuccess)
        {
            return NoContent();
        }

        return NotFound(result.ErrorMessage);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetContainerById(int id)
    {
        var result = await _containerService.GetContainerByIdAsync(id);

        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }

        return NotFound(result.ErrorMessage);
    }

    [HttpGet]
    public async Task<ActionResult> GetContainers()
    {
        var result = await _containerService.GetContainersAsync();

        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }

        return NotFound(result.ErrorMessage);
    }

    [HttpPost("{id}/products")]
    public async Task<IActionResult> AddProducts(int id, AddProductsToContainerDto dto)
    {
        var result = await _containerService.AddProductsToContainerAsync(id, dto);

        if (result.IsSuccess)
        {
            return Ok("Products were added to container");
        }

        return BadRequest(result.ErrorMessage);
    }
}
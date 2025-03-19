using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Warehouse.Data;
using Warehouse.DTOs;
using Warehouse.Entities;
using Warehouse.Enums;
using Warehouse.Services.Interfaces;
using Clothes = Warehouse.Entities.Clothes;
using Food = Warehouse.Entities.Food;

namespace Warehouse.Controller;

[Route("v1/products"), AllowAnonymous]
public class ProductsApiController : BaseApiController
{
    private readonly IProductService _productService;

    public ProductsApiController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult> GetProducts()
    {
        var products = await _productService.GetProductsAsync();

        return Ok(products.Data);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateProduct(int id, UpdateProductDto productDto)
    {
        var product = await _productService.UpdateProduct(id, productDto);

        if (product.IsSuccess)
        {
            return Ok(product);
        }

        return BadRequest(product.ErrorMessage);
    }

    [HttpPost]
    public async Task<ActionResult> AddProduct(AddProductDto productDto)
    {
        var result = await _productService.AddProduct(productDto);

        if (result.IsSuccess)
        {
            return CreatedAtAction(nameof(GetProduct), new { id = result.Data.Id }, result.Data);
        }

        return BadRequest(result.ErrorMessage);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetProduct(int id)
    {
        var result = await _productService.GetProductsByIdAsync(id);

        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }

        return NotFound(result.ErrorMessage);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var result = await _productService.RemoveProduct(id);

        if (result.IsSuccess)
        {
            return Ok();
        }

        return BadRequest(result.ErrorMessage);
    }

    [HttpPost("test-products")]
    public async Task<ActionResult> AddTestProducts()
    {
        var result = await _productService.AddTestProducts();

        if (result.IsSuccess)
        {
            return Ok(_productService.GetProductsAsync());
        }

        return BadRequest(result.ErrorMessage);
    }
}
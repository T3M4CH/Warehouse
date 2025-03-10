using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Warehouse.Data;
using Warehouse.Entities;
using Warehouse.Products;
using Food = Warehouse.Entities.Food;

namespace Warehouse.Controller;

[ApiController]
[Route("[controller]")]
public class ProductsApiController : ControllerBase
{
    private readonly DataContext _dataContext;

    public ProductsApiController(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    [HttpGet]
    public async Task<ActionResult> GetProducts()
    {
        var list = await _dataContext.Products.ToListAsync();

        return Ok(list);
    }

    [HttpPut]
    public async Task<ActionResult> AddTestProducts()
    {
        if (await _dataContext.Products.AnyAsync()) return new BadRequestResult();

        Animal[] animals =
        {
            new()
            {
                Name = "Snake",
                Weight = 50.0,
                PassId = "1241x15125",
            },

            new()
            {
                Name = "BigDawg",
                Weight = 30.0,
                PassId = "0x1241ad12",
            }
        };

        await _dataContext.Animals.AddRangeAsync(animals);

        Cloth[] clothes =
        {
            new()
            {
                Name = "GucciGlasses",
                Weight = 0.2,
                Size = "M"
            },
            new()
            {
                Name = "T-Shirt Stone Island",
                Weight = 2.0,
                Size = "XXXL"
            },
        };

        await _dataContext.Clothes.AddRangeAsync(clothes);

        Food[] foods =
        {
            new()
            {
                Name = "Snacks",
                Weight = 0.5,
                ExpiredData = new DateTime(2025, 3, 10).ToUniversalTime(),
            },

            new()
            {
                Name = "GodCheese",
                Weight = 0.2,
                ExpiredData = new DateTime(2025, 3, 15).ToUniversalTime(),
            },

            new()
            {
                Name = "ExpiredFood",
                Weight = 0.2,
                ExpiredData = new DateTime(2025, 2, 1).ToUniversalTime(),
            },
        };

        await _dataContext.Foods.AddRangeAsync(foods);
        await _dataContext.SaveChangesAsync();

        return Ok(_dataContext.Products);
    }
}
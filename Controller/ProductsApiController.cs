using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Warehouse.Data;
using Warehouse.Entities;
using Warehouse.Enums;
using Clothes = Warehouse.Entities.Clothes;
using Food = Warehouse.Entities.Food;

namespace Warehouse.Controller;

public class ProductsApiController : BaseApiController
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
                Category = EProductType.Animals
            },

            new()
            {
                Name = "BigDawg",
                Weight = 30.0,
                PassId = "0x1241ad12",
                Category = EProductType.Animals
            }
        };

        await _dataContext.Animals.AddRangeAsync(animals);

        Clothes[] clothes =
        {
            new()
            {
                Name = "GucciGlasses",
                Weight = 0.2,
                Size = "M",
                Category = EProductType.Clothes

            },
            new()
            {
                Name = "T-Shirt Stone Island",
                Weight = 2.0,
                Size = "XXXL",
                Category = EProductType.Animals

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
                Category = EProductType.Food

            },

            new()
            {
                Name = "GodCheese",
                Weight = 0.2,
                ExpiredData = new DateTime(2025, 3, 15).ToUniversalTime(),
                Category = EProductType.Food
            },

            new()
            {
                Name = "ExpiredFood",
                Weight = 0.2,
                ExpiredData = new DateTime(2025, 2, 1).ToUniversalTime(),
                Category = EProductType.Food
            },
        };

        await _dataContext.Foods.AddRangeAsync(foods);
        await _dataContext.SaveChangesAsync();

        return Ok(_dataContext.Products);
    }
}
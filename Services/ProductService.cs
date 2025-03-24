using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Warehouse.Data;
using Warehouse.DTOs;
using Warehouse.Entities;
using Warehouse.Enums;
using Warehouse.Helpers;
using Warehouse.Services.Interfaces;
using Warehouse.UnitOfWork.Interfaces;

namespace Warehouse.Services;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<OperationResult<IEnumerable<ProductEntity>>> GetProductsAsync()
    {
        var products = await _unitOfWork.ProductRepository.GetProductsAsync();
        return OperationResult<IEnumerable<ProductEntity>>.Success(products);
    }

    public async Task<OperationResult<ProductEntity?>> GetProductsByIdAsync(int id)
    {
        var product = await _unitOfWork.ProductRepository.GetProductByIdAsync(id);

        if (product == null)
        {
            return OperationResult<ProductEntity?>.Failure("Not found product with ID " + id);
        }

        return OperationResult<ProductEntity?>.Success(product);
    }

    public async Task<OperationResult<ProductEntity?>> UpdateProduct(int id, UpdateProductDto productDto)
    {
        var product = await _unitOfWork.ProductRepository.GetProductByIdAsync(id);

        if (product == null) return OperationResult<ProductEntity?>.Failure($"Product with ID {id} not found");

        product.Name = productDto.Name ?? product.Name;
        product.Weight = productDto.Weight ?? product.Weight;

        switch (product)
        {
            case FoodEntity food:
                if (productDto.ExpiryDate != null)
                {
                    food.ExpiredData = productDto.ExpiryDate.Value.ToUniversalTime();
                }

                break;
            case AnimalEntity animal:
                animal.PassId = productDto.PassId ?? animal.PassId;
                break;
            case ClothesEntity clothes:
                clothes.Size = productDto.Size ?? clothes.Size;
                break;
            default:
                return OperationResult<ProductEntity?>.Failure("Invalid product category.");
        }

        await _unitOfWork.CommitAsync();

        return OperationResult<ProductEntity?>.Success(product);
    }

    public async Task<OperationResult<ProductEntity>> AddProduct(AddProductDto productDto)
    {
        ProductEntity productEntity;

        switch (productDto.Category)
        {
            case EProductType.Animals:
                if (string.IsNullOrWhiteSpace(productDto.PassId))
                    return OperationResult<ProductEntity>.Failure("PassId is required for animals.");

                productEntity = new AnimalEntity
                {
                    Name = productDto.Name,
                    Weight = productDto.Weight,
                    Category = productDto.Category,
                    PassId = productDto.PassId
                };
                break;

            case EProductType.Clothes:
                if (string.IsNullOrWhiteSpace(productDto.Size))
                    return OperationResult<ProductEntity>.Failure("Size is required for clothes.");

                productEntity = new ClothesEntity
                {
                    Name = productDto.Name,
                    Weight = productDto.Weight,
                    Category = productDto.Category,
                    Size = productDto.Size
                };
                break;

            case EProductType.Food:
                if (!productDto.ExpiryDate.HasValue)
                    return OperationResult<ProductEntity>.Failure("Expiry date is required for food.");

                if (productDto.ExpiryDate.Value <= DateTime.UtcNow)
                    return OperationResult<ProductEntity>.Failure("Expiry date must be in the future.");

                productEntity = new FoodEntity
                {
                    Name = productDto.Name,
                    Weight = productDto.Weight,
                    Category = productDto.Category,
                    ExpiredData = productDto.ExpiryDate.Value.ToUniversalTime()
                };
                break;

            default:
                return OperationResult<ProductEntity>.Failure("Invalid category.");
        }

        await _unitOfWork.ProductRepository.AddProduct(productEntity);
        await _unitOfWork.CommitAsync();

        return OperationResult<ProductEntity>.Success(productEntity);
    }

    public async Task<OperationResult> RemoveProduct(int id)
    {
        var product = await _unitOfWork.ProductRepository.GetProductByIdAsync(id);
        if (product == null) return OperationResult.Failure($"Product with ID {id} not found");

        if (product.ContainerId.HasValue) return OperationResult.Failure($"Product placed in container {product.ContainerId.Value} you can't remove it");

        _unitOfWork.ProductRepository.RemoveProduct(product);
        await _unitOfWork.CommitAsync();

        return OperationResult.Success();
    }

    public async Task<OperationResult> AddTestProducts()
    {
        if (await _unitOfWork.ProductRepository.AnyAsync()) return OperationResult.Failure("Already added");

        AnimalEntity[] animals =
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

        ClothesEntity[] clothes =
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

        FoodEntity[] foods =
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

        var products = animals.Cast<ProductEntity>().Concat(clothes).Concat(foods);

        await _unitOfWork.ProductRepository.AddProducts(products);
        await _unitOfWork.CommitAsync();

        return OperationResult.Success();
    }
}
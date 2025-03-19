using Microsoft.AspNetCore.Http.HttpResults;
using Warehouse.Containers;
using Warehouse.DTOs;
using Warehouse.Entities;
using Warehouse.Enums;
using Warehouse.Helpers;
using Warehouse.Services.Interfaces;
using Warehouse.UnitOfWork.Interfaces;

namespace Warehouse.Services;

public class ContainerService : IContainerService
{
    private readonly IUnitOfWork _unitOfWork;

    public ContainerService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<OperationResult<Container?>> CreateContainerAsync(CreateContainerDto containerDto)
    {
        if (containerDto.WarehouseId.HasValue)
        {
            var warehouseExists = await _unitOfWork.WarehouseRepository.ExistByIdAsync(containerDto.WarehouseId.Value);
            if (!warehouseExists)
                return OperationResult<Container?>.Failure($"Warehouse with ID {containerDto.WarehouseId.Value} does not exist.");
        }

        var container = new Container()
        {
            MaxWeight = containerDto.MaxWeight,
            Type = containerDto.Type,
            Category = containerDto.Category,
            WarehouseId = containerDto.WarehouseId
        };

        await _unitOfWork.ContainerRepository.AddAsync(container);
        await _unitOfWork.CommitAsync();

        return OperationResult<Container?>.Success(container);
    }

    public async Task<OperationResult> DeleteContainerAsync(int id)
    {
        var container = await _unitOfWork.ContainerRepository.GetByIdAsync(id);

        if (container == null)
        {
            return OperationResult.Failure($"Not found container with ID {id}");
        }

        await _unitOfWork.ContainerRepository.RemoveAsync(container);
        return OperationResult.Success();
    }

    public async Task<OperationResult<Container?>> GetContainerByIdAsync(int id)
    {
        var container = await _unitOfWork.ContainerRepository.GetByIdAsync(id);

        if (container == null)
        {
            return OperationResult<Container?>.Failure("Container not found");
        }

        return OperationResult<Container?>.Success(container);
    }

    public async Task<OperationResult<IEnumerable<Container>>> GetContainersAsync()
    {
        var containers = await _unitOfWork.ContainerRepository.GetContainersWithProductsAsync();

        if (containers.Any())
        {
            return OperationResult<IEnumerable<Container>>.Success(containers);
        }

        return OperationResult<IEnumerable<Container>>.Failure("Empty container list");
    }

    public async Task<OperationResult> AddProductsToContainerAsync(int containerId, AddProductsToContainerDto dto)
    {
        var container = await _unitOfWork.ContainerRepository.GetByIdAsync(containerId);
        if (container == null)
            return OperationResult.Failure($"Container with ID {containerId} is not found");

        var products = await _unitOfWork.ProductRepository.GetProductsListByIdsAsync(dto.ProductIds);
        var missingProducts = dto.ProductIds.Except(products.Select(p => p.Id)).ToList();

        if (missingProducts.Any())
            return OperationResult.Failure($"Products with ID {string.Join(", ", missingProducts)} not found.");

        var differentCategoryProducts = products
            .Where(p => dto.ProductIds.Contains(p.Id) && p.Category != container.Category)
            .ToList();

        if (differentCategoryProducts.Any())
            return OperationResult.Failure($"{string.Join(", ", differentCategoryProducts.Select(x => $"{x.Name} - {x.Category}"))} is not the same category as the container {container.Category}.");

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
            return OperationResult.Failure($"Error with adding product: {ex.Message}");
        }

        container.Products.AddRange(products);
        await _unitOfWork.CommitAsync();

        return OperationResult.Success();
    }
}
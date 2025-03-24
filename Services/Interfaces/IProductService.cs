using Warehouse.DTOs;
using Warehouse.Entities;
using Warehouse.Helpers;

namespace Warehouse.Services.Interfaces;

public interface IProductService
{
    Task<OperationResult<IEnumerable<ProductEntity>>> GetProductsAsync();
    Task<OperationResult<ProductEntity?>> GetProductsByIdAsync(int id);
    Task<OperationResult<ProductEntity?>> UpdateProduct(int id, UpdateProductDto productDto);

    Task<OperationResult<ProductEntity>> AddProduct(AddProductDto productDto);

    Task<OperationResult> RemoveProduct(int id);

    Task<OperationResult> AddTestProducts();
}
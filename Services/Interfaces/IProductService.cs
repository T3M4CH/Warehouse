using Warehouse.DTOs;
using Warehouse.Entities;
using Warehouse.Helpers;

namespace Warehouse.Services.Interfaces;

public interface IProductService
{
    Task<OperationResult<IEnumerable<Product>>> GetProductsAsync();
    Task<OperationResult<Product?>> GetProductsByIdAsync(int id);
    Task<OperationResult<Product?>> UpdateProduct(int id, UpdateProductDto productDto);

    Task<OperationResult<Product>> AddProduct(AddProductDto productDto);

    Task<OperationResult> RemoveProduct(int id);

    Task<OperationResult> AddTestProducts();
}
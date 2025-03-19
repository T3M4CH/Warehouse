using Warehouse.DTOs;
using Warehouse.Entities;
using Warehouse.Helpers;

namespace Warehouse.Services.Interfaces;

public interface IContainerService
{
    Task<OperationResult<Container?>> CreateContainerAsync(CreateContainerDto containerDto); //
    Task<OperationResult> DeleteContainerAsync(int id); //
    Task<OperationResult<Container?>> GetContainerByIdAsync(int id); //
    Task<OperationResult<IEnumerable<Container>>> GetContainersAsync();
    Task<OperationResult> AddProductsToContainerAsync(int id, AddProductsToContainerDto dto);
}
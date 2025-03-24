using Warehouse.DTOs;
using Warehouse.Entities;
using Warehouse.Helpers;

namespace Warehouse.Services.Interfaces;

public interface IContainerService
{
    Task<OperationResult<ContainerEntity?>> CreateContainerAsync(CreateContainerDto containerDto); //
    Task<OperationResult> DeleteContainerAsync(int id); //
    Task<OperationResult<ContainerEntity?>> GetContainerByIdAsync(int id); //
    Task<OperationResult<IEnumerable<ContainerEntity>>> GetContainersAsync();
    Task<OperationResult> AddProductsToContainerAsync(int id, AddProductsToContainerDto dto);
}
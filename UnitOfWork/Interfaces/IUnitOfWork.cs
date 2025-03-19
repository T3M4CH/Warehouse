using WarehouseApi.Repositories.Interfaces;

namespace Warehouse.UnitOfWork.Interfaces;

public interface IUnitOfWork
{
    Task CommitAsync();
    
    IProductRepository ProductRepository { get; }
    IContainerRepository ContainerRepository { get; }
    IWarehouseRepository WarehouseRepository { get; }
}
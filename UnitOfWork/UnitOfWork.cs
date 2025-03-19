using Warehouse.Data;
using Warehouse.UnitOfWork.Interfaces;
using WarehouseApi.Repositories.Interfaces;

namespace Warehouse.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly DataContext _context;

    public UnitOfWork
    (
        DataContext context,
        IProductRepository productRepository,
        IContainerRepository containerRepository,
        IWarehouseRepository warehouseRepository
    )
    {
        _context = context;
        ProductRepository = productRepository;
        ContainerRepository = containerRepository;
        WarehouseRepository = warehouseRepository;
    }

    public async Task CommitAsync()
    {
        await _context.SaveChangesAsync();
    }

    public IProductRepository ProductRepository { get; }
    public IContainerRepository ContainerRepository { get; }
    public IWarehouseRepository WarehouseRepository { get; }
}
using System.Collections;
using Microsoft.EntityFrameworkCore;
using Warehouse.Data;
using Warehouse.Entities;
using WarehouseApi.Repositories.Interfaces;

namespace WarehouseApi.Repositories;

public class ContainerRepository : IContainerRepository
{
    private readonly DataContext _dataContext;

    public ContainerRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<Container?> GetByIdAsync(int id)
    {
        return await _dataContext.Containers.Include(c => c.Products).FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Container>> GetByWarehouseIdAsync(int warehouseId)
    {
        return await _dataContext.Containers.Where(c => c.WarehouseId == warehouseId).ToListAsync();
    }

    public async Task<List<Container>> GetContainersWithProductsAsync()
    {
        return await _dataContext.Containers.Include(c => c.Products).ToListAsync();
    }

    public async Task AddAsync(Container container)
    {
        await _dataContext.Containers.AddAsync(container);
    }

    public async Task RemoveAsync(Container container)
    {
        _dataContext.Containers.Remove(container);
        await _dataContext.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _dataContext.Containers.AnyAsync(c => c.Id == id);
    }

    public Task AddProductsAsync(int containerId, ICollection<int> productIds)
    {
        return null;
    }
}
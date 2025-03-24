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

    public async Task<ContainerEntity?> GetByIdAsync(int id)
    {
        return await _dataContext.Containers.Include(c => c.Products).FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<ContainerEntity>> GetByWarehouseIdAsync(int warehouseId)
    {
        return await _dataContext.Containers.Where(c => c.WarehouseId == warehouseId).ToListAsync();
    }

    public async Task<List<ContainerEntity>> GetContainersWithProductsAsync()
    {
        return await _dataContext.Containers.Include(c => c.Products).ToListAsync();
    }

    public async Task AddAsync(ContainerEntity containerEntity)
    {
        await _dataContext.Containers.AddAsync(containerEntity);
    }

    public async Task RemoveAsync(ContainerEntity containerEntity)
    {
        _dataContext.Containers.Remove(containerEntity);
        await _dataContext.SaveChangesAsync();
    }
}
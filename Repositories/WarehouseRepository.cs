using Microsoft.EntityFrameworkCore;
using Warehouse.Data;
using WarehouseApi.Entities;
using WarehouseApi.Repositories.Interfaces;

namespace WarehouseApi.Repositories;

public class WarehouseRepository : IWarehouseRepository
{
    private readonly DataContext _context;

    public WarehouseRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<WarehouseEntity>> GetAllWithContainersAsync()
    {
        return await _context.Warehouses
            .Include(w => w.Containers)
            .ToListAsync();
    }

    public async Task<bool> ExistByIdAsync(int id)
    {
        return await _context.Warehouses.AnyAsync(w => w.Id == id);
    }

    public async Task<WarehouseEntity?> GetByIdAsync(int id)
    {
        return await _context.Warehouses
            .Include(w => w.Containers)
            .FirstOrDefaultAsync(w => w.Id == id);
    }

    public async Task AddAsync(WarehouseEntity warehouse)
    {
        await _context.Warehouses.AddAsync(warehouse);
    }

    public async Task<double> GetTotalWeightAsync(int warehouseId)
    {
        return await _context.Containers
            .Where(c => c.WarehouseId == warehouseId)
            .SumAsync(c => c.MaxWeight);
    }
}
using Microsoft.EntityFrameworkCore;
using Warehouse.Data;
using Warehouse.Entities;
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

    public async Task<bool> BelongsToUser(string userId, int warehouseId)
    {
        var user = await _context.Users.Include(u => u.UserWarehouses).ThenInclude(uw => uw.WarehouseEntity).FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            return false;
        }

        var warehouseBelongsToUser = user.UserWarehouses.Any(uw => uw.WarehouseId == warehouseId);

        return warehouseBelongsToUser;
    }

    public async Task<WarehouseEntity> CreateWarehouse(WarehouseEntity warehouse, string userId)
    {
        var id = warehouse.Id;
        await _context.Warehouses.AddAsync(warehouse);

        id = warehouse.Id;
        await _context.UserWarehouses.AddAsync(new UserWarehouseEntity { UserId = userId, WarehouseId = warehouse.Id });

        return warehouse;
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
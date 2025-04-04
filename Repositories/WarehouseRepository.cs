using Microsoft.EntityFrameworkCore;
using Warehouse.Data;
using Warehouse.Entities;
using WarehouseApi.Entities;
using WarehouseApi.Repositories.Interfaces;

namespace WarehouseApi.Repositories;

public class WarehouseRepository : IWarehouseRepository, IDisposable, IAsyncDisposable
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
        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            await _context.Warehouses.AddAsync(warehouse);

            await _context.SaveChangesAsync();

            await _context.UserWarehouses.AddAsync(new UserWarehouseEntity
            {
                UserId = userId,
                WarehouseId = warehouse.Id
            });
            
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return warehouse;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
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

    public void Dispose()
    {
        _context.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
    }
}
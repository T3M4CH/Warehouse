using Microsoft.AspNetCore.Identity;

namespace Warehouse.Entities;

public record UserWarehouse
{
    public required string UserId { get; set; }
    public IdentityUser User { get; set; }

    public int WarehouseId { get; set; }
    public WarehouseApi.Entities.WarehouseEntity WarehouseEntity { get; set; }
}
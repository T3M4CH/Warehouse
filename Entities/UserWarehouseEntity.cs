using Microsoft.AspNetCore.Identity;
using WarehouseApi.Entities;

namespace Warehouse.Entities;

public record UserWarehouseEntity
{
    public required string UserId { get; set; }
    public UserEntity? User { get; set; }

    public int WarehouseId { get; set; }
    public WarehouseEntity? WarehouseEntity { get; set; }
}
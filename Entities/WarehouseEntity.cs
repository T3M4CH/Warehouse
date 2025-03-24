using Microsoft.AspNetCore.Identity;
using Warehouse.Entities;

namespace WarehouseApi.Entities;

public class WarehouseEntity
{
    public int Id { get; set; }
    public required string Location { get; set; }
    public ICollection<ContainerEntity> Containers { get; set; } = new List<ContainerEntity>();
    public ICollection<UserWarehouseEntity> UserWarehouses { get; set; } = new List<UserWarehouseEntity>();
}
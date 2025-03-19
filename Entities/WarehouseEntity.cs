using Microsoft.AspNetCore.Identity;
using Warehouse.Entities;

namespace WarehouseApi.Entities;

public class WarehouseEntity
{
    public int Id { get; set; }
    public required string Location { get; set; }

    public string? UserId { get; set; }
    public IdentityUser User { get; set; }

    public List<Container> Containers { get; set; } = new();
}
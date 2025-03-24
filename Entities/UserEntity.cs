using Microsoft.AspNetCore.Identity;
using Warehouse.Entities;

namespace WarehouseApi.Entities;

public class UserEntity : IdentityUser
{
    public ICollection<UserWarehouseEntity> UserWarehouses { get; set; } = new List<UserWarehouseEntity>();
}
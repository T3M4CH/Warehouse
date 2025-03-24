using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Warehouse.Entities;

namespace WarehouseApi.Entities;

public class UserEntity : IdentityUser
{
    [JsonIgnore]
    public ICollection<UserWarehouseEntity> UserWarehouses { get; set; } = new List<UserWarehouseEntity>();
}
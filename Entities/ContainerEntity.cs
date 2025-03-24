using Warehouse.Enums;
using WarehouseApi.Entities;

namespace Warehouse.Entities;

public class ContainerEntity
{
    public int Id { get; set; }
    public double MaxWeight { get; set; }
    public EContainerType Type { get; set; }
    public EProductType Category { get; set; }
    public int? WarehouseId { get; set; }
    public WarehouseEntity? WarehouseEntity { get; set; }

    public List<ProductEntity> Products { get; set; } = new List<ProductEntity>();
}
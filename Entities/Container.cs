using Warehouse.Enums;

namespace Warehouse.Entities;

public class Container
{
    public int Id { get; set; }
    public double MaxWeight { get; set; }
    public EContainerType Type { get; set; }
    public EProductType Category { get; set; }
    public int? WarehouseId { get; set; }

    public List<Product> Products { get; set; } = new List<Product>();
}
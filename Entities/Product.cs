using Warehouse.Enums;

namespace Warehouse.Entities;

public abstract class Product
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required double Weight { get; set; }

    public EProductType Category { get; set; }

    public int? ContainerId { get; set; }
}
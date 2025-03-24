using Warehouse.Enums;

namespace Warehouse.Entities;

public abstract class ProductEntity
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required double Weight { get; set; }

    public EProductType Category { get; set; }

    public int? ContainerId { get; set; }
    public ContainerEntity? Container { get; set; }
}
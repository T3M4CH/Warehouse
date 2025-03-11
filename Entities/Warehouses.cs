namespace Warehouse.Entities;

public class Warehouses
{
    public int Id { get; set; }
    public required string Location { get; set; }

    public List<Container> Containers { get; set; } = new();
}
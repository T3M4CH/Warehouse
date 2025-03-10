namespace Warehouse.Entities;

public class Product
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public double Weight { get; set; }
}
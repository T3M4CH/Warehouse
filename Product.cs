namespace Warehouse;

public class Product
{
    protected Product(string name, double weight)
    {
        Name = name;
        Weight = weight;
    }

    public readonly string Name;
    public readonly double Weight;
}
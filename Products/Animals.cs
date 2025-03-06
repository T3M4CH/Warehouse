namespace Warehouse.Products;

public class Animals : Product
{
    public readonly string PassId;

    public Animals(string name, double weight, string passId) : base(name, weight)
    {
        PassId = passId;
    }
}
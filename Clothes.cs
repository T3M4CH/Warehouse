namespace Warehouse;

public class Clothes : Product
{
    public readonly string Size;

    public Clothes(string name, double weight, string size) : base(name, weight)
    {
        Size = size;
    }
}
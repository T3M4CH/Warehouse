namespace Warehouse;

public class Food : Product
{
    public readonly DateTime ExpireDate;

    public Food(string name, double weight, DateTime expireDate) : base(name, weight)
    {
        ExpireDate = expireDate;
    }
}
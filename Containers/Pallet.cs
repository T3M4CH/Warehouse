using Warehouse.Entities;

namespace Warehouse.Containers;

public class Pallet : Box
{
    private readonly double _minProductWeight;

    public Pallet(double maxWeight, double minProductWeight) : base(maxWeight)
    {
        _minProductWeight = minProductWeight;
    }

    public override void AddProduct(IEnumerable<Product> products)
    {
        if (products.Any(p => p.Weight < _minProductWeight))
        {
            throw new Exception("The product is too light");
        }
    }
}
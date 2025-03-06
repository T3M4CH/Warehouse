using Warehouse.Products;

namespace Warehouse.Containers;

public class Pallet<T> : Box<T> where T : Product
{
    private readonly double _minProductWeight;

    public Pallet(double maxWeight, double minProductWeight) : base(maxWeight)
    {
        _minProductWeight = minProductWeight;
    }

    public override void AddProduct(T product)
    {
        if (product.Weight < _minProductWeight)
        {
            throw new Exception("The product is too light");
        }

        base.AddProduct(product);
    }
}
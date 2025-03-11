using Warehouse.Entities;
using Warehouse.Interfaces;

namespace Warehouse.Containers;

public class Box : IContainerService
{
    private const int Size = 10;

    private int _currentSize;
    private double _currentWeight;
    private readonly double _maxWeight;

    public Box(double maxWeight)
    {
        _maxWeight = maxWeight;
    }

    public virtual void AddProduct(IEnumerable<Product> products)
    {
        if (GetTotalWeight(products) > _maxWeight)
        {
            throw new Exception("Container overweight");
        }

        if (products.Count() > Size)
        {
            throw new Exception("Container's capacity end");
        }

    }

    // Product IContainerService<pRO>.GetProduct(int id)
    // {
    //     try
    //     {
    //         return _products[id];
    //     }
    //     catch (Exception e)
    //     {
    //         Console.WriteLine(e);
    //         return null;
    //     }
    // }

    // public int GetCount() => _products.Count;
    //
    // public bool IsEmpty() => !_products.Any();
    //
    // public IEnumerable<Product> GetProducts() => _products;

    public double GetTotalWeight(IEnumerable<Product> products) => products.Sum(product => product.Weight);
}
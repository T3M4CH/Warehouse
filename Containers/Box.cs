using Warehouse.Interfaces;
using Warehouse.Products;

namespace Warehouse.Containers;

public class Box<T> : IContainerService<T> where T : Product
{
    private const int Size = 10;

    private int _currentSize;
    private double _currentWeight ;
    private readonly double _maxWeight;

    private readonly List<T> _products = new();

    public Box(double maxWeight)
    {
        _maxWeight = maxWeight;
    }

    public virtual void AddProduct(T product)
    {
        if (product.Weight + _currentWeight > _maxWeight)
        {
            throw new Exception("Container overweight");
        }

        if (_currentSize + 1 > Size)
        {
            throw new Exception("Container's capacity end");
        }

        _currentSize += 1;
        _currentWeight += product.Weight;
        _products.Add(product);
    }

    T? IContainerService<T>.GetProduct(int id)
    {
        try
        {
            return _products[id];
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    public int GetCount() => _products.Count;

    public bool IsEmpty() => !_products.Any();
    
    public IEnumerable<Product> GetProducts() => _products;

    public double GetTotalWeight() => _products.Sum(product => product.Weight);
}
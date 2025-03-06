using Warehouse.Interfaces;
using Warehouse.Products;

namespace Warehouse.Controller;

public class Warehouse
{
    private List<IContainerBase> _containers = new();

    public void AddContainer<T>(IContainerService<T> container) where T : Product
    {
        _containers.Add(container);
    }

    public double GetTotalWeight() => _containers.Sum(container => container.GetTotalWeight());

    public void SortContainersByWeight() => _containers = _containers.OrderBy(c => c.GetTotalWeight()).ToList();

    public IEnumerable<Product> FilterProducts(Func<Product, bool> filterFunc)
    {
        return _containers.SelectMany(container => container.GetProducts())
            .Where(filterFunc);
    }

    public IEnumerable<IContainerService<T>> GetContainersByType<T>() where T : Product
    {
        return _containers.OfType<IContainerService<T>>();
    }
}
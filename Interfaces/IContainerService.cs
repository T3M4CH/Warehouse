using Warehouse.Products;

namespace Warehouse.Interfaces;

public interface IContainerService<T> : IContainerBase where T : Product
{
    void AddProduct(T product);
    T? GetProduct(int id); 
}
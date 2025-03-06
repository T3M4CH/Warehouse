using Warehouse.Products;

namespace Warehouse.Interfaces;

public interface IContainerBase
{
    double GetTotalWeight();
    int GetCount();
    bool IsEmpty();
    IEnumerable<Product> GetProducts();
}
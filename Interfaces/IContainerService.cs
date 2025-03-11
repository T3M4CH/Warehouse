using Warehouse.Entities;

namespace Warehouse.Interfaces;

public interface IContainerService
{
    //Product GetProduct(int id);
    void AddProduct(IEnumerable<Product> products);
    // double GetTotalWeight();
    // int GetCount();
    // bool IsEmpty();
    // IEnumerable<Product> GetProducts();
}
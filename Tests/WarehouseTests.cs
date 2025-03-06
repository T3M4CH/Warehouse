using Warehouse.Containers;
using Warehouse.Products;
using Xunit;

public class WarehouseTests
{
    private Warehouse.Controller.Warehouse _warehouse;

    [Fact]
    public void TestTotalWeight()
    {
        _warehouse = new Warehouse.Controller.Warehouse();

        var container = new Box<Food>(50);
        container.AddProduct(new Food("Chips", 50.0, DateTime.Now));

        _warehouse.AddContainer(container);

        Assert.Equal(50.0, _warehouse.GetTotalWeight());
    }

    [Fact]
    public void TestOverweight()
    {
        var container = new Pallet<Animals>(50.0, 25.0);

        Exception exception = null;
        try
        {
            container.AddProduct(new Animals("Kot", 600.0, "1231"));
        }
        catch (Exception ex)
        {
            exception = ex;
        }

        Assert.NotNull(exception);
        Assert.Equal("Container overweight", exception.Message);
    }

    [Fact]
    public void TestCapacityLimit()
    {
        var container = new Box<Animals>(50.0);

        Exception exception = null;
        try
        {
            for (int i = 0; i < 20; i++)
            {
                container.AddProduct(new Animals("Kot", 1.0, "1231"));
            }
        }
        catch (Exception ex)
        {
            exception = ex;
        }

        Assert.NotNull(exception);
        Assert.Equal("Container's capacity end", exception.Message);
    }

    [Fact]
    public void TestResultsFilter()
    {
        _warehouse = new Warehouse.Controller.Warehouse();

        var container = new Box<Food>(50);
        container.AddProduct(new Food("Chips", 20.0, DateTime.Now));

        _warehouse.AddContainer(container);

        var filterResults = _warehouse.FilterProducts(product => product.Name.Contains("Krokodil"));

        Assert.False(filterResults.Any());

        container.AddProduct(new Food("Krokodil :))", 10, DateTime.Today));

        filterResults = _warehouse.FilterProducts(product => product.Name.Contains("Krokodil"));

        Assert.True(filterResults.Any());
    }
}
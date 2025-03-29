using Moq;
using Warehouse.DTOs;
using Warehouse.Entities;
using Warehouse.Enums;
using Warehouse.Services;
using Warehouse.UnitOfWork.Interfaces;
using WarehouseApi.Repositories.Interfaces;
using Xunit;

public class ProductServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly ProductService _productService;

    public ProductServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockProductRepository = new Mock<IProductRepository>();

        _mockUnitOfWork.Setup(u => u.ProductRepository).Returns(_mockProductRepository.Object);

        _productService = new ProductService(_mockUnitOfWork.Object);
    }

    [Fact]
    public async Task GetProductsAsync_ShouldReturnSuccess_WhenProductsExist()
    {
        var expectedProducts = new List<ProductEntity>
        {
            new AnimalEntity { Id = 1, Name = "Product1", Weight = 10, PassId = "xasa" },
            new FoodEntity { Id = 2, Name = "Product2", Weight = 20, ExpiredData = DateTime.UtcNow }
        };

        _mockProductRepository.Setup(r => r.GetProductsAsync())
            .ReturnsAsync(expectedProducts);

        var result = await _productService.GetProductsAsync();

        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Data.Count());
    }

    [Theory]
    [InlineData("Snake", 50.0, "1241x15125", null, null, EProductType.Animals, true)] // OK
    [InlineData("T-Shirt Stone Island", 2.0, null, "XXXL", null, EProductType.Clothes, true)] // OK
    [InlineData("Cheese", 0.5, "M", null, null, EProductType.Food, false)] // Bad
    [InlineData("ExpiredFood", 0.2, "x123", "m", "2025-01-01", EProductType.Food, false)] // Bad
    public async Task AddProduct_ShouldReturnSuccess_WhenValidDataIsProvided(string name, double weight, string passId, string size, string? expiryDate, EProductType category, bool expectedResult)
    {
        DateTime? expiry = string.IsNullOrEmpty(expiryDate) ? null : DateTime.Parse(expiryDate);

        var productDto = new AddProductDto
        {
            Name = name,
            Weight = weight,
            PassId = passId,
            Category = category,
            Size = size,
            ExpiryDate = expiry
        };

        _mockProductRepository.Setup(repo => repo.AddProduct(It.IsAny<ProductEntity>())).Returns(Task.CompletedTask);

        var result = await _productService.AddProduct(productDto);

        Assert.Equal(expectedResult, result.IsSuccess);
    }

    [Fact]
    public async Task GetProductsByIdAsync_ShouldReturnProduct_WhenProductExists()
    {
        var productId = 1;
        var expectedProduct = new ClothesEntity { Id = productId, Name = "Product1", Weight = 10, Size = "M" };

        _mockProductRepository.Setup(r => r.GetProductByIdAsync(productId))
            .ReturnsAsync(expectedProduct);

        var result = await _productService.GetProductsByIdAsync(productId);

        Assert.True(result.IsSuccess);
        Assert.Equal(expectedProduct, result.Data);
    }

    [Fact]
    public async Task GetProductsByIdAsync_ShouldReturnFailure_WhenProductNotFound()
    {
        var productId = 1;

        _mockProductRepository.Setup(r => r.GetProductByIdAsync(productId))
            .ReturnsAsync((ProductEntity?)null);

        var result = await _productService.GetProductsByIdAsync(productId);

        Assert.False(result.IsSuccess);
        Assert.Equal("Not found product with ID 1", result.ErrorMessage);
    }

    [Fact]
    public async Task AddProduct_ShouldReturnSuccess_WhenProductIsAdded()
    {
        var addProductDto = new AddProductDto
        {
            Name = "New Product",
            Weight = 10,
            Category = EProductType.Animals,
            PassId = "12345"
        };
        var addedProduct = new AnimalEntity { Name = "New Product", Weight = 10, PassId = "12345", Category = EProductType.Animals };

        _mockProductRepository.Setup(r => r.AddProduct(It.IsAny<ProductEntity>())).Verifiable();
        _mockUnitOfWork.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);

        var result = await _productService.AddProduct(addProductDto);

        Assert.True(result.IsSuccess);
        _mockProductRepository.Verify(r => r.AddProduct(It.IsAny<ProductEntity>()), Times.Once);
        _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task RemoveProduct_ShouldReturnFailure_WhenProductNotFound()
    {
        var productId = 1;

        _mockProductRepository.Setup(r => r.GetProductByIdAsync(productId))
            .ReturnsAsync((ProductEntity?)null);

        var result = await _productService.RemoveProduct(productId);

        Assert.False(result.IsSuccess);
        Assert.Equal("Product with ID 1 not found", result.ErrorMessage);
    }

    [Fact]
    public async Task RemoveProduct_ShouldReturnFailure_WhenProductInContainer()
    {
        var productId = 1;
        var product = new AnimalEntity
        {
            Id = productId,
            ContainerId = 1,
            Name = "Krisa",
            Weight = 5,
            Category = EProductType.Animals,
            PassId = "XAsda",
        };

        _mockProductRepository.Setup(r => r.GetProductByIdAsync(productId))
            .ReturnsAsync(product);

        var result = await _productService.RemoveProduct(productId);

        Assert.False(result.IsSuccess);
        Assert.Equal("Product placed in container 1 you can't remove it", result.ErrorMessage);
    }

    [Fact]
    public async Task RemoveProduct_ShouldReturnSuccess_WhenProductRemoved()
    {
        var productId = 1;
        var product = new AnimalEntity
        {
            Id = productId,
            PassId = "Xas",
            Name = "ASDa",
            Weight = 0
        };

        _mockProductRepository.Setup(r => r.GetProductByIdAsync(productId))
            .ReturnsAsync(product);

        _mockProductRepository.Setup(r => r.RemoveProduct(product)).Verifiable();
        _mockUnitOfWork.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);

        var result = await _productService.RemoveProduct(productId);

        Assert.True(result.IsSuccess);
        _mockProductRepository.Verify(r => r.RemoveProduct(product), Times.Once);
        _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task AddTestProducts_ShouldReturnFailure_WhenProductsAlreadyAdded()
    {
        _mockProductRepository.Setup(r => r.AnyAsync())
            .ReturnsAsync(true);

        var result = await _productService.AddTestProducts();

        Assert.False(result.IsSuccess);
        Assert.Equal("Already added", result.ErrorMessage);
    }

    [Fact]
    public async Task AddTestProducts_ShouldReturnSuccess_WhenProductsAdded()
    {
        _mockProductRepository.Setup(r => r.AnyAsync())
            .ReturnsAsync(false);

        _mockProductRepository.Setup(r => r.AddProducts(It.IsAny<IEnumerable<ProductEntity>>())).Verifiable();
        _mockUnitOfWork.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);

        var result = await _productService.AddTestProducts();

        Assert.True(result.IsSuccess);
        _mockProductRepository.Verify(r => r.AddProducts(It.IsAny<IEnumerable<ProductEntity>>()), Times.Once);
        _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task AddProduct_ShouldReturnFailure_WhenCategoryIsInvalid()
    {
        var productDto = new AddProductDto
        {
            Name = "InvalidProduct",
            Weight = 10.0,
            Category = (EProductType)999 // Invalid category
        };

        var result = await _productService.AddProduct(productDto);

        Assert.False(result.IsSuccess);
        Assert.Equal("Invalid category.", result.ErrorMessage);
    }
}
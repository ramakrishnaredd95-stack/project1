using FlipkartApp.API.Services;
using FlipkartApp.Core.DTOs;
using FlipkartApp.Core.Entities;
using FlipkartApp.Core.Enums;
using FlipkartApp.Core.Interfaces;
using Moq;

namespace FlipkartApp.API.Tests.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _mockRepo;
        private readonly ProductService _service;

        public ProductServiceTests()
        {
            _mockRepo = new Mock<IProductRepository>();
            _service = new ProductService(_mockRepo.Object);
        }

        private static Product CreateSampleProduct(int id = 1, ProductType type = ProductType.Mobile) => new()
        {
            Id = id,
            Name = $"Test Product {id}",
            Description = "Test Description",
            Price = 999,
            DiscountPrice = 899,
            ImageUrl = "/test.jpg",
            SKU = $"SKU-{id}",
            Brand = "TestBrand",
            StockQuantity = 10,
            Rating = 4.5,
            ReviewCount = 100,
            ProductType = type,
            CategoryId = 1,
            IsActive = true,
            Category = new Category { Id = 1, Name = "Test Category", ProductType = type }
        };

        [Fact]
        public async Task GetAllProductsAsync_ReturnsAllProducts()
        {
            var products = new List<Product> { CreateSampleProduct(1), CreateSampleProduct(2) };
            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(products);

            var result = await _service.GetAllProductsAsync();

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetProductByIdAsync_ExistingId_ReturnsProduct()
        {
            var product = CreateSampleProduct();
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);

            var result = await _service.GetProductByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal("Test Product 1", result.Name);
        }

        [Fact]
        public async Task GetProductByIdAsync_NonExistingId_ReturnsNull()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Product?)null);

            var result = await _service.GetProductByIdAsync(999);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetMobileProductsAsync_ReturnsOnlyMobileProducts()
        {
            var products = new List<Product>
            {
                CreateSampleProduct(1, ProductType.Mobile),
                CreateSampleProduct(2, ProductType.Mobile)
            };
            _mockRepo.Setup(r => r.GetByTypeAsync(ProductType.Mobile)).ReturnsAsync(products);

            var result = await _service.GetMobileProductsAsync();

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetGroceryProductsAsync_ReturnsOnlyGroceryProducts()
        {
            var products = new List<Product> { CreateSampleProduct(1, ProductType.Grocery) };
            _mockRepo.Setup(r => r.GetByTypeAsync(ProductType.Grocery)).ReturnsAsync(products);

            var result = await _service.GetGroceryProductsAsync();

            Assert.Single(result);
        }

        [Fact]
        public async Task SearchProductsAsync_ReturnsMatchingProducts()
        {
            var products = new List<Product> { CreateSampleProduct() };
            _mockRepo.Setup(r => r.SearchAsync("Test")).ReturnsAsync(products);

            var result = await _service.SearchProductsAsync("Test");

            Assert.Single(result);
        }

        [Fact]
        public async Task CreateProductAsync_ReturnsCreatedProduct()
        {
            var dto = new CreateProductDto
            {
                Name = "New Product",
                Description = "New Description",
                Price = 500,
                DiscountPrice = 450,
                Brand = "NewBrand",
                SKU = "NEW-001",
                StockQuantity = 20,
                ProductType = ProductType.Mobile,
                CategoryId = 1
            };

            _mockRepo.Setup(r => r.CreateAsync(It.IsAny<Product>()))
                .ReturnsAsync((Product p) =>
                {
                    p.Id = 1;
                    p.Category = new Category { Id = 1, Name = "Test" };
                    return p;
                });

            var result = await _service.CreateProductAsync(dto);

            Assert.Equal("New Product", result.Name);
            Assert.Equal(500, result.Price);
        }

        [Fact]
        public async Task UpdateProductAsync_ExistingProduct_ReturnsUpdated()
        {
            var product = CreateSampleProduct();
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);
            _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Product>())).ReturnsAsync(product);

            var dto = new UpdateProductDto
            {
                Name = "Updated Product",
                Price = 1099,
                DiscountPrice = 999,
                Brand = "Updated Brand",
                StockQuantity = 5,
                IsActive = true
            };

            var result = await _service.UpdateProductAsync(1, dto);

            Assert.NotNull(result);
            _mockRepo.Verify(r => r.UpdateAsync(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public async Task UpdateProductAsync_NonExisting_ReturnsNull()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Product?)null);

            var result = await _service.UpdateProductAsync(999, new UpdateProductDto());

            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteProductAsync_ExistingProduct_ReturnsTrue()
        {
            _mockRepo.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

            var result = await _service.DeleteProductAsync(1);

            Assert.True(result);
        }

        [Fact]
        public async Task DeleteProductAsync_NonExisting_ReturnsFalse()
        {
            _mockRepo.Setup(r => r.DeleteAsync(999)).ReturnsAsync(false);

            var result = await _service.DeleteProductAsync(999);

            Assert.False(result);
        }

        [Fact]
        public async Task GetProductsByCategoryAsync_ReturnsProducts()
        {
            var products = new List<Product> { CreateSampleProduct() };
            _mockRepo.Setup(r => r.GetByCategoryAsync(1)).ReturnsAsync(products);

            var result = await _service.GetProductsByCategoryAsync(1);

            Assert.Single(result);
        }
    }
}

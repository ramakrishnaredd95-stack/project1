using FlipkartApp.Core.Entities;
using FlipkartApp.Core.Enums;
using FlipkartApp.Infrastructure.Data;
using FlipkartApp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FlipkartApp.Infrastructure.Tests.Repositories
{
    public class ProductRepositoryTests : IDisposable
    {
        private readonly FlipkartDbContext _context;
        private readonly ProductRepository _repository;

        public ProductRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<FlipkartDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new FlipkartDbContext(options);
            _repository = new ProductRepository(_context);

            SeedTestData();
        }

        private void SeedTestData()
        {
            var category1 = new Category { Id = 100, Name = "Smartphones", ProductType = ProductType.Mobile };
            var category2 = new Category { Id = 101, Name = "Fruits", ProductType = ProductType.Grocery };

            _context.Categories.AddRange(category1, category2);

            _context.Products.AddRange(
                new Product { Id = 100, Name = "Galaxy S24", Brand = "Samsung", Price = 999, DiscountPrice = 899, SKU = "TEST-001", ProductType = ProductType.Mobile, CategoryId = 100, IsActive = true, Category = category1 },
                new Product { Id = 101, Name = "iPhone 15", Brand = "Apple", Price = 1099, DiscountPrice = 999, SKU = "TEST-002", ProductType = ProductType.Mobile, CategoryId = 100, IsActive = true, Category = category1 },
                new Product { Id = 102, Name = "Fresh Bananas", Brand = "FreshFarm", Price = 50, DiscountPrice = 45, SKU = "TEST-003", ProductType = ProductType.Grocery, CategoryId = 101, IsActive = true, Category = category2 },
                new Product { Id = 103, Name = "Deleted Product", Brand = "Old", Price = 100, DiscountPrice = 90, SKU = "TEST-004", ProductType = ProductType.Mobile, CategoryId = 100, IsActive = false, Category = category1 }
            );

            _context.SaveChanges();
        }

        [Fact]
        public async Task GetAllAsync_ReturnsOnlyActiveProducts()
        {
            var result = await _repository.GetAllAsync();
            Assert.Equal(3, result.Count()); // Excludes inactive product
        }

        [Fact]
        public async Task GetByIdAsync_ExistingId_ReturnsProduct()
        {
            var result = await _repository.GetByIdAsync(100);
            Assert.NotNull(result);
            Assert.Equal("Galaxy S24", result.Name);
        }

        [Fact]
        public async Task GetByTypeAsync_Mobile_ReturnsMobileProducts()
        {
            var result = await _repository.GetByTypeAsync(ProductType.Mobile);
            Assert.Equal(2, result.Count()); // 2 active mobile products
        }

        [Fact]
        public async Task GetByTypeAsync_Grocery_ReturnsGroceryProducts()
        {
            var result = await _repository.GetByTypeAsync(ProductType.Grocery);
            Assert.Single(result);
        }

        [Fact]
        public async Task SearchAsync_MatchingTerm_ReturnsResults()
        {
            var result = await _repository.SearchAsync("Samsung");
            Assert.Single(result);
            Assert.Equal("Galaxy S24", result.First().Name);
        }

        [Fact]
        public async Task SearchAsync_NoMatch_ReturnsEmpty()
        {
            var result = await _repository.SearchAsync("NonExistent");
            Assert.Empty(result);
        }

        [Fact]
        public async Task DeleteAsync_SoftDeletes()
        {
            var result = await _repository.DeleteAsync(100);
            Assert.True(result);

            var product = await _context.Products.FindAsync(100);
            Assert.False(product!.IsActive); // Soft deleted
        }

        [Fact]
        public async Task GetByCategoryAsync_ReturnsProductsInCategory()
        {
            var result = await _repository.GetByCategoryAsync(100);
            Assert.Equal(2, result.Count());
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

using FlipkartApp.Core.Entities;
using FlipkartApp.Core.Enums;
using FlipkartApp.Infrastructure.Data;
using FlipkartApp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FlipkartApp.Infrastructure.Tests.Repositories
{
    public class CategoryRepositoryTests : IDisposable
    {
        private readonly FlipkartDbContext _context;
        private readonly CategoryRepository _repository;

        public CategoryRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<FlipkartDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new FlipkartDbContext(options);
            _repository = new CategoryRepository(_context);
            SeedData();
        }

        private void SeedData()
        {
            _context.Categories.AddRange(
                new Category { Id = 1, Name = "Smartphones", Description = "Phones", ProductType = ProductType.Mobile,
                    Products = new List<Product>
                    {
                        new() { Id = 1, Name = "Galaxy S24", SKU = "SKU-1", Price = 999, DiscountPrice = 899, ProductType = ProductType.Mobile, IsActive = true }
                    }
                },
                new Category { Id = 2, Name = "Tablets", Description = "Tablet devices", ProductType = ProductType.Mobile, Products = new List<Product>() },
                new Category { Id = 3, Name = "Fruits", Description = "Fresh fruits", ProductType = ProductType.Grocery,
                    Products = new List<Product>
                    {
                        new() { Id = 2, Name = "Bananas", SKU = "SKU-2", Price = 50, DiscountPrice = 45, ProductType = ProductType.Grocery, IsActive = true }
                    }
                }
            );
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllCategories()
        {
            var result = await _repository.GetAllAsync();
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ExistingId_ReturnsCategory()
        {
            var result = await _repository.GetByIdAsync(1);
            Assert.NotNull(result);
            Assert.Equal("Smartphones", result.Name);
        }

        [Fact]
        public async Task GetByIdAsync_NonExisting_ReturnsNull()
        {
            var result = await _repository.GetByIdAsync(999);
            Assert.Null(result);
        }

        [Fact]
        public async Task GetByTypeAsync_Mobile_ReturnsMobileCategories()
        {
            var result = await _repository.GetByTypeAsync(ProductType.Mobile);
            Assert.Equal(2, result.Count());
            Assert.All(result, c => Assert.Equal(ProductType.Mobile, c.ProductType));
        }

        [Fact]
        public async Task GetByTypeAsync_Grocery_ReturnsGroceryCategories()
        {
            var result = await _repository.GetByTypeAsync(ProductType.Grocery);
            Assert.Single(result);
            Assert.Equal("Fruits", result.First().Name);
        }

        [Fact]
        public async Task GetByIdAsync_IncludesProducts()
        {
            var result = await _repository.GetByIdAsync(1);
            Assert.NotNull(result);
            Assert.NotNull(result.Products);
            Assert.Single(result.Products);
        }

        public void Dispose() => _context.Dispose();
    }
}

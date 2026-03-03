using FlipkartApp.API.Services;
using FlipkartApp.Core.DTOs;
using FlipkartApp.Core.Entities;
using FlipkartApp.Core.Enums;
using FlipkartApp.Core.Interfaces;
using Moq;

namespace FlipkartApp.API.Tests.Services
{
    public class CategoryServiceTests
    {
        private readonly Mock<ICategoryRepository> _mockRepo;
        private readonly CategoryService _service;

        public CategoryServiceTests()
        {
            _mockRepo = new Mock<ICategoryRepository>();
            _service = new CategoryService(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAllCategoriesAsync_ReturnsAllCategories()
        {
            var categories = new List<Category>
            {
                new() { Id = 1, Name = "Smartphones", ProductType = ProductType.Mobile, Products = new List<Product> { new() { Id = 1, IsActive = true } } },
                new() { Id = 2, Name = "Grocery", ProductType = ProductType.Grocery, Products = new List<Product>() }
            };
            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(categories);

            var result = await _service.GetAllCategoriesAsync();

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetCategoryByIdAsync_ExistingId_ReturnsCategory()
        {
            var category = new Category { Id = 1, Name = "Smartphones", ProductType = ProductType.Mobile, Products = new List<Product>() };
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(category);

            var result = await _service.GetCategoryByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal("Smartphones", result.Name);
        }

        [Fact]
        public async Task GetCategoryByIdAsync_NonExisting_ReturnsNull()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Category?)null);

            var result = await _service.GetCategoryByIdAsync(999);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetCategoriesByTypeAsync_Mobile_ReturnsMobileCategories()
        {
            var categories = new List<Category>
            {
                new() { Id = 1, Name = "Smartphones", ProductType = ProductType.Mobile, Products = new List<Product>() }
            };
            _mockRepo.Setup(r => r.GetByTypeAsync(ProductType.Mobile)).ReturnsAsync(categories);

            var result = await _service.GetCategoriesByTypeAsync(ProductType.Mobile);

            Assert.Single(result);
            Assert.Equal(ProductType.Mobile, result.First().ProductType);
        }

        [Fact]
        public async Task GetCategoriesByTypeAsync_Grocery_ReturnsGroceryCategories()
        {
            var categories = new List<Category>
            {
                new() { Id = 5, Name = "Fruits", ProductType = ProductType.Grocery, Products = new List<Product>() }
            };
            _mockRepo.Setup(r => r.GetByTypeAsync(ProductType.Grocery)).ReturnsAsync(categories);

            var result = await _service.GetCategoriesByTypeAsync(ProductType.Grocery);

            Assert.Single(result);
        }

        [Fact]
        public async Task GetAllCategoriesAsync_IncludesProductCount()
        {
            var categories = new List<Category>
            {
                new() { Id = 1, Name = "Phones", ProductType = ProductType.Mobile, Products = new List<Product> { new(), new(), new() } }
            };
            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(categories);

            var result = await _service.GetAllCategoriesAsync();
            var first = result.First();

            Assert.Equal(3, first.ProductCount);
        }
    }
}

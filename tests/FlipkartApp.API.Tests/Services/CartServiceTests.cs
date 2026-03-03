using FlipkartApp.API.Services;
using FlipkartApp.Core.DTOs;
using FlipkartApp.Core.Entities;
using FlipkartApp.Core.Enums;
using FlipkartApp.Core.Interfaces;
using Moq;

namespace FlipkartApp.API.Tests.Services
{
    public class CartServiceTests
    {
        private readonly Mock<ICartRepository> _mockCartRepo;
        private readonly Mock<IProductRepository> _mockProductRepo;
        private readonly CartService _service;

        public CartServiceTests()
        {
            _mockCartRepo = new Mock<ICartRepository>();
            _mockProductRepo = new Mock<IProductRepository>();
            _service = new CartService(_mockCartRepo.Object, _mockProductRepo.Object);
        }

        [Fact]
        public async Task GetCartAsync_ReturnsCartItems()
        {
            var items = new List<CartItem>
            {
                new() { Id = 1, SessionId = "sess1", ProductId = 1, Quantity = 2, Product = new Product { Id = 1, Name = "Test", Price = 100, DiscountPrice = 90 } }
            };
            _mockCartRepo.Setup(r => r.GetBySessionIdAsync("sess1")).ReturnsAsync(items);

            var result = await _service.GetCartAsync("sess1");

            Assert.Single(result);
            Assert.Equal("Test", result.First().ProductName);
        }

        [Fact]
        public async Task AddToCartAsync_ValidProduct_ReturnsCartItem()
        {
            var product = new Product { Id = 1, Name = "Test", Price = 100, DiscountPrice = 90 };
            _mockProductRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);

            var cartItem = new CartItem { Id = 1, SessionId = "sess1", ProductId = 1, Quantity = 1, Product = product };
            _mockCartRepo.Setup(r => r.AddAsync(It.IsAny<CartItem>())).ReturnsAsync(cartItem);
            _mockCartRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(cartItem);

            var result = await _service.AddToCartAsync("sess1", new AddToCartDto { ProductId = 1, Quantity = 1 });

            Assert.NotNull(result);
            Assert.Equal(1, result.ProductId);
        }

        [Fact]
        public async Task AddToCartAsync_InvalidProduct_ThrowsArgumentException()
        {
            _mockProductRepo.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Product?)null);

            await Assert.ThrowsAsync<ArgumentException>(() =>
                _service.AddToCartAsync("sess1", new AddToCartDto { ProductId = 999 }));
        }

        [Fact]
        public async Task UpdateCartItemAsync_ExistingItem_ReturnsUpdated()
        {
            var item = new CartItem { Id = 1, SessionId = "sess1", ProductId = 1, Quantity = 1, Product = new Product { Name = "Test", Price = 100, DiscountPrice = 90 } };
            _mockCartRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(item);
            _mockCartRepo.Setup(r => r.UpdateAsync(It.IsAny<CartItem>())).ReturnsAsync(item);

            var result = await _service.UpdateCartItemAsync(1, new UpdateCartDto { Quantity = 3 });

            Assert.NotNull(result);
        }

        [Fact]
        public async Task UpdateCartItemAsync_NonExisting_ReturnsNull()
        {
            _mockCartRepo.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((CartItem?)null);

            var result = await _service.UpdateCartItemAsync(999, new UpdateCartDto { Quantity = 3 });

            Assert.Null(result);
        }

        [Fact]
        public async Task RemoveFromCartAsync_ExistingItem_ReturnsTrue()
        {
            _mockCartRepo.Setup(r => r.RemoveAsync(1)).ReturnsAsync(true);

            var result = await _service.RemoveFromCartAsync(1);

            Assert.True(result);
        }

        [Fact]
        public async Task ClearCartAsync_ReturnsTrue()
        {
            _mockCartRepo.Setup(r => r.ClearCartAsync("sess1")).ReturnsAsync(true);

            var result = await _service.ClearCartAsync("sess1");

            Assert.True(result);
        }
    }
}

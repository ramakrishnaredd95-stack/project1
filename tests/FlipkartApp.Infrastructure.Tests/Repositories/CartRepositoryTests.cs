using FlipkartApp.Core.Entities;
using FlipkartApp.Core.Enums;
using FlipkartApp.Infrastructure.Data;
using FlipkartApp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FlipkartApp.Infrastructure.Tests.Repositories
{
    public class CartRepositoryTests : IDisposable
    {
        private readonly FlipkartDbContext _context;
        private readonly CartRepository _repository;
        private const string SessionA = "session-A";
        private const string SessionB = "session-B";

        public CartRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<FlipkartDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new FlipkartDbContext(options);
            _repository = new CartRepository(_context);
            SeedData();
        }

        private void SeedData()
        {
            var product1 = new Product { Id = 10, Name = "Phone", SKU = "P1", Price = 500, DiscountPrice = 450, ProductType = ProductType.Mobile, IsActive = true };
            var product2 = new Product { Id = 11, Name = "Rice", SKU = "P2", Price = 100, DiscountPrice = 90, ProductType = ProductType.Grocery, IsActive = true };
            _context.Products.AddRange(product1, product2);

            _context.CartItems.AddRange(
                new CartItem { Id = 1, SessionId = SessionA, ProductId = 10, Quantity = 2 },
                new CartItem { Id = 2, SessionId = SessionA, ProductId = 11, Quantity = 1 },
                new CartItem { Id = 3, SessionId = SessionB, ProductId = 10, Quantity = 1 }
            );
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetBySessionIdAsync_ReturnsOnlySessionItems()
        {
            var result = await _repository.GetBySessionIdAsync(SessionA);
            Assert.Equal(2, result.Count());
            Assert.All(result, item => Assert.Equal(SessionA, item.SessionId));
        }

        [Fact]
        public async Task GetBySessionIdAsync_DifferentSession_ReturnsOwnItems()
        {
            var result = await _repository.GetBySessionIdAsync(SessionB);
            Assert.Single(result);
        }

        [Fact]
        public async Task GetByIdAsync_ExistingItem_ReturnsItem()
        {
            var result = await _repository.GetByIdAsync(1);
            Assert.NotNull(result);
            Assert.Equal(SessionA, result.SessionId);
        }

        [Fact]
        public async Task GetByIdAsync_NonExisting_ReturnsNull()
        {
            var result = await _repository.GetByIdAsync(999);
            Assert.Null(result);
        }

        [Fact]
        public async Task AddAsync_NewItem_AddsToCart()
        {
            var newItem = new CartItem { SessionId = "new-session", ProductId = 10, Quantity = 3 };
            var result = await _repository.AddAsync(newItem);

            Assert.NotEqual(0, result.Id);
            var inDb = await _context.CartItems.FindAsync(result.Id);
            Assert.NotNull(inDb);
            Assert.Equal(3, inDb.Quantity);
        }

        [Fact]
        public async Task AddAsync_ExistingProductInSession_MergesQuantity()
        {
            // Product 10 already exists in SessionA with quantity 2 — adding 3 more should merge to 5
            var newItem = new CartItem { SessionId = SessionA, ProductId = 10, Quantity = 3 };
            await _repository.AddAsync(newItem);

            var existing = await _context.CartItems.FirstAsync(c => c.SessionId == SessionA && c.ProductId == 10);
            Assert.Equal(5, existing.Quantity);
        }

        [Fact]
        public async Task UpdateAsync_QuantityChanges()
        {
            var item = await _context.CartItems.FindAsync(1);
            item!.Quantity = 10;
            var result = await _repository.UpdateAsync(item);
            Assert.Equal(10, result.Quantity);
        }

        [Fact]
        public async Task RemoveAsync_ExistingItem_RemovesFromDb()
        {
            var result = await _repository.RemoveAsync(1);
            Assert.True(result);
            var inDb = await _context.CartItems.FindAsync(1);
            Assert.Null(inDb);
        }

        [Fact]
        public async Task RemoveAsync_NonExisting_ReturnsFalse()
        {
            var result = await _repository.RemoveAsync(999);
            Assert.False(result);
        }

        [Fact]
        public async Task ClearCartAsync_RemovesAllSessionItems()
        {
            var result = await _repository.ClearCartAsync(SessionA);
            Assert.True(result);

            var remaining = await _context.CartItems.Where(c => c.SessionId == SessionA).ToListAsync();
            Assert.Empty(remaining);

            // SessionB items should still exist
            var sessionB = await _context.CartItems.Where(c => c.SessionId == SessionB).ToListAsync();
            Assert.Single(sessionB);
        }

        public void Dispose() => _context.Dispose();
    }
}

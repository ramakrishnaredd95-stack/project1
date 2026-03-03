using FlipkartApp.Core.Entities;
using FlipkartApp.Core.Enums;
using FlipkartApp.Infrastructure.Data;
using FlipkartApp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FlipkartApp.Infrastructure.Tests.Repositories
{
    public class OrderRepositoryTests : IDisposable
    {
        private readonly FlipkartDbContext _context;
        private readonly OrderRepository _repository;

        public OrderRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<FlipkartDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new FlipkartDbContext(options);
            _repository = new OrderRepository(_context);
            SeedData();
        }

        private void SeedData()
        {
            var product = new Product { Id = 20, Name = "Test Phone", SKU = "TP-1", Price = 500, DiscountPrice = 450, ProductType = ProductType.Mobile, IsActive = true };
            _context.Products.Add(product);

            _context.Orders.AddRange(
                new Order
                {
                    Id = 1,
                    OrderNumber = "FK-20240101-AABB1234",
                    CustomerName = "Alice",
                    CustomerEmail = "alice@test.com",
                    ShippingAddress = "123 Main St",
                    TotalAmount = 900,
                    Status = OrderStatus.Pending,
                    OrderItems = new List<OrderItem>
                    {
                        new() { ProductId = 20, ProductName = "Test Phone", UnitPrice = 450, Quantity = 2 }
                    }
                },
                new Order
                {
                    Id = 2,
                    OrderNumber = "FK-20240101-CCDD5678",
                    CustomerName = "Bob",
                    CustomerEmail = "bob@test.com",
                    ShippingAddress = "456 Oak Ave",
                    TotalAmount = 450,
                    Status = OrderStatus.Confirmed,
                    OrderItems = new List<OrderItem>
                    {
                        new() { ProductId = 20, ProductName = "Test Phone", UnitPrice = 450, Quantity = 1 }
                    }
                }
            );
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllOrders()
        {
            var result = await _repository.GetAllAsync();
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ExistingOrder_ReturnsWithItems()
        {
            var result = await _repository.GetByIdAsync(1);
            Assert.NotNull(result);
            Assert.Equal("Alice", result.CustomerName);
            Assert.NotNull(result.OrderItems);
            Assert.Single(result.OrderItems);
        }

        [Fact]
        public async Task GetByIdAsync_NonExisting_ReturnsNull()
        {
            var result = await _repository.GetByIdAsync(999);
            Assert.Null(result);
        }

        [Fact]
        public async Task GetByOrderNumberAsync_ExistingNumber_ReturnsOrder()
        {
            var result = await _repository.GetByOrderNumberAsync("FK-20240101-AABB1234");
            Assert.NotNull(result);
            Assert.Equal("Alice", result.CustomerName);
        }

        [Fact]
        public async Task GetByOrderNumberAsync_NonExisting_ReturnsNull()
        {
            var result = await _repository.GetByOrderNumberAsync("FK-DOES-NOT-EXIST");
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateAsync_AddsOrderToDb()
        {
            var newOrder = new Order
            {
                OrderNumber = "FK-20240101-NEW99999",
                CustomerName = "Charlie",
                CustomerEmail = "charlie@test.com",
                ShippingAddress = "789 Pine Rd",
                TotalAmount = 450,
                Status = OrderStatus.Pending,
                OrderItems = new List<OrderItem>
                {
                    new() { ProductId = 20, ProductName = "Test Phone", UnitPrice = 450, Quantity = 1 }
                }
            };

            var result = await _repository.CreateAsync(newOrder);

            Assert.NotEqual(0, result.Id);
            var inDb = await _context.Orders.FindAsync(result.Id);
            Assert.NotNull(inDb);
            Assert.Equal("Charlie", inDb.CustomerName);
        }

        [Fact]
        public async Task UpdateAsync_StatusChanged_SavesCorrectly()
        {
            var order = await _context.Orders.FindAsync(1);
            order!.Status = OrderStatus.Shipped;
            var result = await _repository.UpdateAsync(order);

            Assert.Equal(OrderStatus.Shipped, result.Status);
            var inDb = await _context.Orders.FindAsync(1);
            Assert.Equal(OrderStatus.Shipped, inDb!.Status);
        }

        [Fact]
        public async Task GetAllAsync_IncludesOrderItems()
        {
            var result = await _repository.GetAllAsync();
            var first = result.First();
            Assert.NotNull(first.OrderItems);
            Assert.NotEmpty(first.OrderItems);
        }

        public void Dispose() => _context.Dispose();
    }
}

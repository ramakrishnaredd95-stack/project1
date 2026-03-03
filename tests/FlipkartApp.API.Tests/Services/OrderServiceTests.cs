using FlipkartApp.API.Services;
using FlipkartApp.Core.DTOs;
using FlipkartApp.Core.Entities;
using FlipkartApp.Core.Enums;
using FlipkartApp.Core.Interfaces;
using Moq;

namespace FlipkartApp.API.Tests.Services
{
    public class OrderServiceTests
    {
        private readonly Mock<IOrderRepository> _mockOrderRepo;
        private readonly Mock<ICartRepository> _mockCartRepo;
        private readonly Mock<IProductRepository> _mockProductRepo;
        private readonly OrderService _service;

        public OrderServiceTests()
        {
            _mockOrderRepo = new Mock<IOrderRepository>();
            _mockCartRepo = new Mock<ICartRepository>();
            _mockProductRepo = new Mock<IProductRepository>();
            _service = new OrderService(_mockOrderRepo.Object, _mockCartRepo.Object, _mockProductRepo.Object);
        }

        [Fact]
        public async Task GetAllOrdersAsync_ReturnsOrders()
        {
            var orders = new List<Order>
            {
                new() { Id = 1, OrderNumber = "FK-001", CustomerName = "Test", TotalAmount = 100, OrderItems = new List<OrderItem>() }
            };
            _mockOrderRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(orders);

            var result = await _service.GetAllOrdersAsync();

            Assert.Single(result);
        }

        [Fact]
        public async Task GetOrderByIdAsync_ExistingOrder_ReturnsOrder()
        {
            var order = new Order { Id = 1, OrderNumber = "FK-001", CustomerName = "Test", TotalAmount = 100, OrderItems = new List<OrderItem>() };
            _mockOrderRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(order);

            var result = await _service.GetOrderByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal("FK-001", result.OrderNumber);
        }

        [Fact]
        public async Task GetOrderByIdAsync_NonExisting_ReturnsNull()
        {
            _mockOrderRepo.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Order?)null);

            var result = await _service.GetOrderByIdAsync(999);

            Assert.Null(result);
        }

        [Fact]
        public async Task CreateOrderAsync_WithCartItems_CreatesOrder()
        {
            var cartItems = new List<CartItem>
            {
                new() { Id = 1, SessionId = "sess1", ProductId = 1, Quantity = 2, Product = new Product { Id = 1, Name = "Phone", Price = 1000, DiscountPrice = 900 } }
            };
            _mockCartRepo.Setup(r => r.GetBySessionIdAsync("sess1")).ReturnsAsync(cartItems);
            _mockOrderRepo.Setup(r => r.CreateAsync(It.IsAny<Order>())).ReturnsAsync((Order o) => { o.Id = 1; return o; });
            _mockCartRepo.Setup(r => r.ClearCartAsync("sess1")).ReturnsAsync(true);

            var dto = new CreateOrderDto { CustomerName = "John", CustomerEmail = "john@test.com", ShippingAddress = "123 Test St", SessionId = "sess1" };
            var result = await _service.CreateOrderAsync(dto);

            Assert.NotNull(result);
            Assert.Equal("John", result.CustomerName);
            Assert.Equal(1800, result.TotalAmount); // 900 * 2
            _mockCartRepo.Verify(r => r.ClearCartAsync("sess1"), Times.Once);
        }

        [Fact]
        public async Task CreateOrderAsync_EmptyCart_ThrowsInvalidOperationException()
        {
            _mockCartRepo.Setup(r => r.GetBySessionIdAsync("empty")).ReturnsAsync(new List<CartItem>());

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.CreateOrderAsync(new CreateOrderDto { SessionId = "empty" }));
        }

        [Fact]
        public async Task UpdateOrderStatusAsync_ExistingOrder_UpdatesStatus()
        {
            var order = new Order { Id = 1, OrderNumber = "FK-001", Status = OrderStatus.Pending, OrderItems = new List<OrderItem>() };
            _mockOrderRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(order);
            _mockOrderRepo.Setup(r => r.UpdateAsync(It.IsAny<Order>())).ReturnsAsync((Order o) => o);

            var result = await _service.UpdateOrderStatusAsync(1, OrderStatus.Shipped);

            Assert.NotNull(result);
            Assert.Equal(OrderStatus.Shipped, result.Status);
        }

        [Fact]
        public async Task UpdateOrderStatusAsync_NonExisting_ReturnsNull()
        {
            _mockOrderRepo.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Order?)null);

            var result = await _service.UpdateOrderStatusAsync(999, OrderStatus.Shipped);

            Assert.Null(result);
        }
    }
}

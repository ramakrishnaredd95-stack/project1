using FlipkartApp.Core.DTOs;
using FlipkartApp.Core.Entities;
using FlipkartApp.Core.Enums;
using FlipkartApp.Core.Interfaces;

namespace FlipkartApp.API.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDto>> GetAllOrdersAsync();
        Task<OrderDto?> GetOrderByIdAsync(int id);
        Task<OrderDto> CreateOrderAsync(CreateOrderDto dto);
        Task<OrderDto?> UpdateOrderStatusAsync(int id, OrderStatus status);
    }

    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;

        public OrderService(IOrderRepository orderRepository, ICartRepository cartRepository, IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAllAsync();
            return orders.Select(MapToDto);
        }

        public async Task<OrderDto?> GetOrderByIdAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            return order == null ? null : MapToDto(order);
        }

        public async Task<OrderDto> CreateOrderAsync(CreateOrderDto dto)
        {
            var cartItems = await _cartRepository.GetBySessionIdAsync(dto.SessionId);
            var cartList = cartItems.ToList();

            if (!cartList.Any())
                throw new InvalidOperationException("Cart is empty");

            var order = new Order
            {
                OrderNumber = $"FK-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..8].ToUpper()}",
                CustomerName = dto.CustomerName,
                CustomerEmail = dto.CustomerEmail,
                ShippingAddress = dto.ShippingAddress,
                Status = OrderStatus.Pending,
                OrderItems = cartList.Select(ci => new OrderItem
                {
                    ProductId = ci.ProductId,
                    ProductName = ci.Product?.Name ?? string.Empty,
                    UnitPrice = ci.Product?.DiscountPrice ?? ci.Product?.Price ?? 0,
                    Quantity = ci.Quantity
                }).ToList()
            };

            order.TotalAmount = order.OrderItems.Sum(oi => oi.UnitPrice * oi.Quantity);

            var created = await _orderRepository.CreateAsync(order);

            // Clear cart
            await _cartRepository.ClearCartAsync(dto.SessionId);

            return MapToDto(created);
        }

        public async Task<OrderDto?> UpdateOrderStatusAsync(int id, OrderStatus status)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null) return null;

            order.Status = status;
            var updated = await _orderRepository.UpdateAsync(order);
            return MapToDto(updated);
        }

        private static OrderDto MapToDto(Order order)
        {
            return new OrderDto
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                CustomerName = order.CustomerName,
                CustomerEmail = order.CustomerEmail,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                OrderDate = order.OrderDate,
                Items = order.OrderItems?.Select(oi => new OrderItemDto
                {
                    ProductId = oi.ProductId,
                    ProductName = oi.ProductName,
                    UnitPrice = oi.UnitPrice,
                    Quantity = oi.Quantity,
                    TotalPrice = oi.UnitPrice * oi.Quantity
                }).ToList() ?? new()
            };
        }
    }
}

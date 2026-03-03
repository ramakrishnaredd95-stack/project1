using FlipkartApp.Core.DTOs;
using FlipkartApp.Core.Entities;
using FlipkartApp.Core.Enums;
using FlipkartApp.Core.Interfaces;

namespace FlipkartApp.API.Services
{
    public interface ICartService
    {
        Task<IEnumerable<CartItemDto>> GetCartAsync(string sessionId);
        Task<CartItemDto> AddToCartAsync(string sessionId, AddToCartDto dto);
        Task<CartItemDto?> UpdateCartItemAsync(int id, UpdateCartDto dto);
        Task<bool> RemoveFromCartAsync(int id);
        Task<bool> ClearCartAsync(string sessionId);
    }

    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;

        public CartService(ICartRepository cartRepository, IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<CartItemDto>> GetCartAsync(string sessionId)
        {
            var items = await _cartRepository.GetBySessionIdAsync(sessionId);
            return items.Select(MapToDto);
        }

        public async Task<CartItemDto> AddToCartAsync(string sessionId, AddToCartDto dto)
        {
            var product = await _productRepository.GetByIdAsync(dto.ProductId);
            if (product == null)
                throw new ArgumentException("Product not found");

            var cartItem = new CartItem
            {
                SessionId = sessionId,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity
            };

            var added = await _cartRepository.AddAsync(cartItem);
            // Reload with Product navigation
            var reloaded = await _cartRepository.GetByIdAsync(added.Id);
            return MapToDto(reloaded!);
        }

        public async Task<CartItemDto?> UpdateCartItemAsync(int id, UpdateCartDto dto)
        {
            var item = await _cartRepository.GetByIdAsync(id);
            if (item == null) return null;

            item.Quantity = dto.Quantity;
            var updated = await _cartRepository.UpdateAsync(item);
            return MapToDto(updated);
        }

        public async Task<bool> RemoveFromCartAsync(int id)
        {
            return await _cartRepository.RemoveAsync(id);
        }

        public async Task<bool> ClearCartAsync(string sessionId)
        {
            return await _cartRepository.ClearCartAsync(sessionId);
        }

        private static CartItemDto MapToDto(CartItem item)
        {
            return new CartItemDto
            {
                Id = item.Id,
                ProductId = item.ProductId,
                ProductName = item.Product?.Name ?? string.Empty,
                ProductImageUrl = item.Product?.ImageUrl ?? string.Empty,
                ProductPrice = item.Product?.DiscountPrice ?? item.Product?.Price ?? 0,
                Quantity = item.Quantity
            };
        }
    }
}

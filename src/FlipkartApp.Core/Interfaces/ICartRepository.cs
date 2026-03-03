using FlipkartApp.Core.Entities;

namespace FlipkartApp.Core.Interfaces
{
    public interface ICartRepository
    {
        Task<IEnumerable<CartItem>> GetBySessionIdAsync(string sessionId);
        Task<CartItem?> GetByIdAsync(int id);
        Task<CartItem> AddAsync(CartItem cartItem);
        Task<CartItem> UpdateAsync(CartItem cartItem);
        Task<bool> RemoveAsync(int id);
        Task<bool> ClearCartAsync(string sessionId);
    }
}

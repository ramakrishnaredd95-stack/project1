using FlipkartApp.Core.Entities;
using FlipkartApp.Core.Interfaces;
using FlipkartApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FlipkartApp.Infrastructure.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly FlipkartDbContext _context;

        public CartRepository(FlipkartDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CartItem>> GetBySessionIdAsync(string sessionId)
        {
            return await _context.CartItems
                .Include(c => c.Product)
                .Where(c => c.SessionId == sessionId)
                .ToListAsync();
        }

        public async Task<CartItem?> GetByIdAsync(int id)
        {
            return await _context.CartItems
                .Include(c => c.Product)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<CartItem> AddAsync(CartItem cartItem)
        {
            var existing = await _context.CartItems
                .FirstOrDefaultAsync(c => c.SessionId == cartItem.SessionId && c.ProductId == cartItem.ProductId);

            if (existing != null)
            {
                existing.Quantity += cartItem.Quantity;
                existing.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                return existing;
            }

            _context.CartItems.Add(cartItem);
            await _context.SaveChangesAsync();
            return cartItem;
        }

        public async Task<CartItem> UpdateAsync(CartItem cartItem)
        {
            cartItem.UpdatedAt = DateTime.UtcNow;
            _context.CartItems.Update(cartItem);
            await _context.SaveChangesAsync();
            return cartItem;
        }

        public async Task<bool> RemoveAsync(int id)
        {
            var item = await _context.CartItems.FindAsync(id);
            if (item == null) return false;

            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ClearCartAsync(string sessionId)
        {
            var items = await _context.CartItems
                .Where(c => c.SessionId == sessionId)
                .ToListAsync();

            _context.CartItems.RemoveRange(items);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

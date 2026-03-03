using FlipkartApp.Core.Entities;
using FlipkartApp.Core.Enums;
using FlipkartApp.Core.Interfaces;
using FlipkartApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FlipkartApp.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly FlipkartDbContext _context;

        public ProductRepository(FlipkartDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products
                .Include(p => p.Category)
                .Where(p => p.IsActive)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Product>> GetByTypeAsync(ProductType type)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Where(p => p.ProductType == type && p.IsActive)
                .OrderByDescending(p => p.Rating)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Where(p => p.CategoryId == categoryId && p.IsActive)
                .OrderByDescending(p => p.Rating)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> SearchAsync(string searchTerm)
        {
            var term = searchTerm.ToLower();
            return await _context.Products
                .Include(p => p.Category)
                .Where(p => p.IsActive &&
                    (p.Name.ToLower().Contains(term) ||
                     p.Description.ToLower().Contains(term) ||
                     p.Brand.ToLower().Contains(term)))
                .OrderByDescending(p => p.Rating)
                .ToListAsync();
        }

        public async Task<Product> CreateAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product> UpdateAsync(Product product)
        {
            product.UpdatedAt = DateTime.UtcNow;
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;

            product.IsActive = false; // Soft delete
            product.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

using FlipkartApp.Core.Entities;
using FlipkartApp.Core.Enums;

namespace FlipkartApp.Core.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(int id);
        Task<IEnumerable<Category>> GetByTypeAsync(ProductType type);
        Task<Category> CreateAsync(Category category);
        Task<Category> UpdateAsync(Category category);
        Task<bool> DeleteAsync(int id);
    }
}

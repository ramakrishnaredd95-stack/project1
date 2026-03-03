using FlipkartApp.Core.DTOs;
using FlipkartApp.Core.Entities;
using FlipkartApp.Core.Enums;
using FlipkartApp.Core.Interfaces;

namespace FlipkartApp.API.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
        Task<CategoryDto?> GetCategoryByIdAsync(int id);
        Task<IEnumerable<CategoryDto>> GetCategoriesByTypeAsync(ProductType type);
    }

    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;

        public CategoryService(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _repository.GetAllAsync();
            return categories.Select(MapToDto);
        }

        public async Task<CategoryDto?> GetCategoryByIdAsync(int id)
        {
            var category = await _repository.GetByIdAsync(id);
            return category == null ? null : MapToDto(category);
        }

        public async Task<IEnumerable<CategoryDto>> GetCategoriesByTypeAsync(ProductType type)
        {
            var categories = await _repository.GetByTypeAsync(type);
            return categories.Select(MapToDto);
        }

        private static CategoryDto MapToDto(Category category)
        {
            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                ImageUrl = category.ImageUrl,
                ProductType = category.ProductType,
                ProductCount = category.Products?.Count ?? 0
            };
        }
    }
}

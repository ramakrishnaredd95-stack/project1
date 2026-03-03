using FlipkartApp.Core.DTOs;
using FlipkartApp.Core.Entities;
using FlipkartApp.Core.Enums;
using FlipkartApp.Core.Interfaces;

namespace FlipkartApp.API.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<ProductDto?> GetProductByIdAsync(int id);
        Task<IEnumerable<ProductDto>> GetMobileProductsAsync();
        Task<IEnumerable<ProductDto>> GetGroceryProductsAsync();
        Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(int categoryId);
        Task<IEnumerable<ProductDto>> SearchProductsAsync(string searchTerm);
        Task<ProductDto> CreateProductAsync(CreateProductDto dto);
        Task<ProductDto?> UpdateProductAsync(int id, UpdateProductDto dto);
        Task<bool> DeleteProductAsync(int id);
    }

    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _repository.GetAllAsync();
            return products.Select(MapToDto);
        }

        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            return product == null ? null : MapToDto(product);
        }

        public async Task<IEnumerable<ProductDto>> GetMobileProductsAsync()
        {
            var products = await _repository.GetByTypeAsync(ProductType.Mobile);
            return products.Select(MapToDto);
        }

        public async Task<IEnumerable<ProductDto>> GetGroceryProductsAsync()
        {
            var products = await _repository.GetByTypeAsync(ProductType.Grocery);
            return products.Select(MapToDto);
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(int categoryId)
        {
            var products = await _repository.GetByCategoryAsync(categoryId);
            return products.Select(MapToDto);
        }

        public async Task<IEnumerable<ProductDto>> SearchProductsAsync(string searchTerm)
        {
            var products = await _repository.SearchAsync(searchTerm);
            return products.Select(MapToDto);
        }

        public async Task<ProductDto> CreateProductAsync(CreateProductDto dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                DiscountPrice = dto.DiscountPrice,
                ImageUrl = dto.ImageUrl,
                SKU = dto.SKU,
                Brand = dto.Brand,
                StockQuantity = dto.StockQuantity,
                ProductType = dto.ProductType,
                Specifications = dto.Specifications,
                CategoryId = dto.CategoryId
            };

            var created = await _repository.CreateAsync(product);
            return MapToDto(created);
        }

        public async Task<ProductDto?> UpdateProductAsync(int id, UpdateProductDto dto)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null) return null;

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.DiscountPrice = dto.DiscountPrice;
            product.ImageUrl = dto.ImageUrl;
            product.Brand = dto.Brand;
            product.StockQuantity = dto.StockQuantity;
            product.Specifications = dto.Specifications;
            product.IsActive = dto.IsActive;

            var updated = await _repository.UpdateAsync(product);
            return MapToDto(updated);
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        private static ProductDto MapToDto(Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                DiscountPrice = product.DiscountPrice,
                ImageUrl = product.ImageUrl,
                Brand = product.Brand,
                StockQuantity = product.StockQuantity,
                Rating = product.Rating,
                ReviewCount = product.ReviewCount,
                ProductType = product.ProductType,
                CategoryName = product.Category?.Name ?? string.Empty,
                CategoryId = product.CategoryId
            };
        }
    }
}

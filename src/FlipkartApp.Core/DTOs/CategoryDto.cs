using FlipkartApp.Core.Enums;

namespace FlipkartApp.Core.DTOs
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public ProductType ProductType { get; set; }
        public int ProductCount { get; set; }
    }
}

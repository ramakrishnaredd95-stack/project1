using FlipkartApp.Core.Enums;

namespace FlipkartApp.Core.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal DiscountPrice { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public int StockQuantity { get; set; }
        public double Rating { get; set; }
        public int ReviewCount { get; set; }
        public ProductType ProductType { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public int CategoryId { get; set; }
    }

    public class CreateProductDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal DiscountPrice { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public int StockQuantity { get; set; }
        public ProductType ProductType { get; set; }
        public string Specifications { get; set; } = string.Empty;
        public int CategoryId { get; set; }
    }

    public class UpdateProductDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal DiscountPrice { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public int StockQuantity { get; set; }
        public string Specifications { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}

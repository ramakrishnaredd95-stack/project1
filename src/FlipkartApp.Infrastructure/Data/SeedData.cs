using FlipkartApp.Core.Entities;
using FlipkartApp.Core.Enums;
using Microsoft.EntityFrameworkCore;

namespace FlipkartApp.Infrastructure.Data
{
    public static class SeedData
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            // Mobile Categories
            var mobileCategories = new[]
            {
                new Category { Id = 1, Name = "Smartphones", Description = "Latest smartphones from top brands", ImageUrl = "/images/categories/smartphones.jpg", ProductType = ProductType.Mobile, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), UpdatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Category { Id = 2, Name = "Tablets", Description = "Tablets for work and entertainment", ImageUrl = "/images/categories/tablets.jpg", ProductType = ProductType.Mobile, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), UpdatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Category { Id = 3, Name = "Mobile Accessories", Description = "Cases, chargers, earphones and more", ImageUrl = "/images/categories/accessories.jpg", ProductType = ProductType.Mobile, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), UpdatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Category { Id = 4, Name = "Wearables", Description = "Smartwatches and fitness bands", ImageUrl = "/images/categories/wearables.jpg", ProductType = ProductType.Mobile, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), UpdatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            };

            // Grocery Categories
            var groceryCategories = new[]
            {
                new Category { Id = 5, Name = "Fruits & Vegetables", Description = "Fresh fruits and vegetables", ImageUrl = "/images/categories/fruits.jpg", ProductType = ProductType.Grocery, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), UpdatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Category { Id = 6, Name = "Dairy & Eggs", Description = "Fresh dairy products and eggs", ImageUrl = "/images/categories/dairy.jpg", ProductType = ProductType.Grocery, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), UpdatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Category { Id = 7, Name = "Snacks & Beverages", Description = "Chips, drinks, and more", ImageUrl = "/images/categories/snacks.jpg", ProductType = ProductType.Grocery, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), UpdatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Category { Id = 8, Name = "Staples", Description = "Rice, flour, oil, and essentials", ImageUrl = "/images/categories/staples.jpg", ProductType = ProductType.Grocery, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), UpdatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            };

            modelBuilder.Entity<Category>().HasData(mobileCategories);
            modelBuilder.Entity<Category>().HasData(groceryCategories);

            // Mobile Products
            var mobileProducts = new[]
            {
                new Product { Id = 1, Name = "Samsung Galaxy S24 Ultra", Description = "Flagship smartphone with S Pen, 200MP camera, Snapdragon 8 Gen 3", Price = 129999, DiscountPrice = 119999, ImageUrl = "/images/products/galaxy-s24.jpg", SKU = "MOB-SAM-S24U", Brand = "Samsung", StockQuantity = 50, Rating = 4.5, ReviewCount = 1234, ProductType = ProductType.Mobile, Specifications = "{\"Display\":\"6.8 inch QHD+\",\"RAM\":\"12GB\",\"Storage\":\"256GB\",\"Battery\":\"5000mAh\"}", CategoryId = 1, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), UpdatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Product { Id = 2, Name = "iPhone 15 Pro Max", Description = "Apple's most powerful iPhone with A17 Pro chip and titanium design", Price = 159900, DiscountPrice = 149900, ImageUrl = "/images/products/iphone-15.jpg", SKU = "MOB-APL-I15PM", Brand = "Apple", StockQuantity = 30, Rating = 4.7, ReviewCount = 2345, ProductType = ProductType.Mobile, Specifications = "{\"Display\":\"6.7 inch OLED\",\"RAM\":\"8GB\",\"Storage\":\"256GB\",\"Battery\":\"4441mAh\"}", CategoryId = 1, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), UpdatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Product { Id = 3, Name = "OnePlus 12", Description = "Performance flagship with Snapdragon 8 Gen 3 and Hasselblad camera", Price = 64999, DiscountPrice = 59999, ImageUrl = "/images/products/oneplus-12.jpg", SKU = "MOB-OP-12", Brand = "OnePlus", StockQuantity = 75, Rating = 4.3, ReviewCount = 987, ProductType = ProductType.Mobile, Specifications = "{\"Display\":\"6.82 inch AMOLED\",\"RAM\":\"12GB\",\"Storage\":\"256GB\",\"Battery\":\"5400mAh\"}", CategoryId = 1, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), UpdatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Product { Id = 4, Name = "iPad Air M2", Description = "Powerful tablet with M2 chip and Liquid Retina display", Price = 74900, DiscountPrice = 69900, ImageUrl = "/images/products/ipad-air.jpg", SKU = "TAB-APL-AIRM2", Brand = "Apple", StockQuantity = 40, Rating = 4.6, ReviewCount = 567, ProductType = ProductType.Mobile, Specifications = "{\"Display\":\"11 inch Liquid Retina\",\"Chip\":\"M2\",\"Storage\":\"128GB\"}", CategoryId = 2, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), UpdatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Product { Id = 5, Name = "Apple Watch Series 9", Description = "Advanced health and fitness smartwatch", Price = 41900, DiscountPrice = 38900, ImageUrl = "/images/products/apple-watch.jpg", SKU = "WR-APL-AW9", Brand = "Apple", StockQuantity = 60, Rating = 4.4, ReviewCount = 789, ProductType = ProductType.Mobile, Specifications = "{\"Display\":\"Always-On Retina\",\"Chip\":\"S9\",\"Battery\":\"18 hours\"}", CategoryId = 4, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), UpdatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Product { Id = 6, Name = "USB-C Fast Charger 65W", Description = "GaN fast charger compatible with all USB-C devices", Price = 1999, DiscountPrice = 1499, ImageUrl = "/images/products/charger.jpg", SKU = "ACC-CHG-65W", Brand = "Anker", StockQuantity = 200, Rating = 4.2, ReviewCount = 456, ProductType = ProductType.Mobile, Specifications = "{\"Power\":\"65W\",\"Ports\":\"2x USB-C\",\"Technology\":\"GaN\"}", CategoryId = 3, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), UpdatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            };

            // Grocery Products
            var groceryProducts = new[]
            {
                new Product { Id = 7, Name = "Fresh Organic Bananas (1 dozen)", Description = "Farm-fresh organic bananas, naturally ripened", Price = 60, DiscountPrice = 49, ImageUrl = "/images/products/bananas.jpg", SKU = "GRO-FV-BAN", Brand = "FreshFarm", StockQuantity = 500, Rating = 4.1, ReviewCount = 234, ProductType = ProductType.Grocery, Specifications = "{\"Weight\":\"1 dozen\",\"Type\":\"Organic\",\"Origin\":\"Karnataka\"}", CategoryId = 5, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), UpdatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Product { Id = 8, Name = "Amul Taza Milk 1L", Description = "Fresh toned milk, pasteurized and homogenized", Price = 54, DiscountPrice = 50, ImageUrl = "/images/products/milk.jpg", SKU = "GRO-DY-MILK", Brand = "Amul", StockQuantity = 300, Rating = 4.3, ReviewCount = 567, ProductType = ProductType.Grocery, Specifications = "{\"Volume\":\"1L\",\"Fat\":\"3%\",\"Type\":\"Toned\"}", CategoryId = 6, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), UpdatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Product { Id = 9, Name = "Lay's Classic Salted Chips 150g", Description = "Crispy potato chips with classic salted flavor", Price = 50, DiscountPrice = 45, ImageUrl = "/images/products/lays.jpg", SKU = "GRO-SN-LAYS", Brand = "Lay's", StockQuantity = 400, Rating = 4.0, ReviewCount = 890, ProductType = ProductType.Grocery, Specifications = "{\"Weight\":\"150g\",\"Flavor\":\"Classic Salted\"}", CategoryId = 7, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), UpdatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Product { Id = 10, Name = "India Gate Basmati Rice 5kg", Description = "Premium aged basmati rice for perfect biryani", Price = 650, DiscountPrice = 599, ImageUrl = "/images/products/rice.jpg", SKU = "GRO-ST-RICE", Brand = "India Gate", StockQuantity = 150, Rating = 4.5, ReviewCount = 1234, ProductType = ProductType.Grocery, Specifications = "{\"Weight\":\"5kg\",\"Type\":\"Basmati\",\"Aging\":\"1 Year\"}", CategoryId = 8, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), UpdatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Product { Id = 11, Name = "Coca-Cola 2L Bottle", Description = "Refreshing carbonated soft drink", Price = 95, DiscountPrice = 85, ImageUrl = "/images/products/coke.jpg", SKU = "GRO-BV-COKE", Brand = "Coca-Cola", StockQuantity = 250, Rating = 4.2, ReviewCount = 345, ProductType = ProductType.Grocery, Specifications = "{\"Volume\":\"2L\",\"Type\":\"Carbonated\"}", CategoryId = 7, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), UpdatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Product { Id = 12, Name = "Fresh Tomatoes 1kg", Description = "Farm-fresh red tomatoes, hand-picked", Price = 40, DiscountPrice = 35, ImageUrl = "/images/products/tomatoes.jpg", SKU = "GRO-FV-TOM", Brand = "FreshFarm", StockQuantity = 350, Rating = 3.9, ReviewCount = 123, ProductType = ProductType.Grocery, Specifications = "{\"Weight\":\"1kg\",\"Type\":\"Fresh\",\"Origin\":\"Maharashtra\"}", CategoryId = 5, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), UpdatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            };

            modelBuilder.Entity<Product>().HasData(mobileProducts);
            modelBuilder.Entity<Product>().HasData(groceryProducts);
        }
    }
}

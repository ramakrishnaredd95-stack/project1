using FlipkartApp.API.Controllers;
using FlipkartApp.API.Services;
using FlipkartApp.Core.DTOs;
using FlipkartApp.Core.Enums;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FlipkartApp.API.Tests.Controllers
{
    public class ProductsControllerTests
    {
        private readonly Mock<IProductService> _mockService;
        private readonly ProductsController _controller;

        public ProductsControllerTests()
        {
            _mockService = new Mock<IProductService>();
            _controller = new ProductsController(_mockService.Object);
        }

        private static ProductDto Sample(int id = 1) => new()
        {
            Id = id, Name = $"Product {id}", Brand = "Brand", Price = 500, DiscountPrice = 450,
            ProductType = ProductType.Mobile, CategoryId = 1, CategoryName = "Phones"
        };

        [Fact]
        public async Task GetAll_ReturnsOkWithProducts()
        {
            _mockService.Setup(s => s.GetAllProductsAsync()).ReturnsAsync(new[] { Sample(1), Sample(2) });
            var result = await _controller.GetAll();
            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var items = Assert.IsAssignableFrom<IEnumerable<ProductDto>>(ok.Value);
            Assert.Equal(2, items.Count());
        }

        [Fact]
        public async Task GetById_ExistingId_ReturnsOk()
        {
            _mockService.Setup(s => s.GetProductByIdAsync(1)).ReturnsAsync(Sample());
            var result = await _controller.GetById(1);
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetById_NonExisting_ReturnsNotFound()
        {
            _mockService.Setup(s => s.GetProductByIdAsync(999)).ReturnsAsync((ProductDto?)null);
            var result = await _controller.GetById(999);
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetMobile_ReturnsOk()
        {
            _mockService.Setup(s => s.GetMobileProductsAsync()).ReturnsAsync(new[] { Sample() });
            var result = await _controller.GetMobile();
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetGrocery_ReturnsOk()
        {
            _mockService.Setup(s => s.GetGroceryProductsAsync()).ReturnsAsync(Array.Empty<ProductDto>());
            var result = await _controller.GetGrocery();
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetByCategory_ReturnsOk()
        {
            _mockService.Setup(s => s.GetProductsByCategoryAsync(1)).ReturnsAsync(new[] { Sample() });
            var result = await _controller.GetByCategory(1);
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task Search_EmptyQuery_ReturnsBadRequest()
        {
            var result = await _controller.Search("   ");
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task Search_ValidQuery_ReturnsOk()
        {
            _mockService.Setup(s => s.SearchProductsAsync("phone")).ReturnsAsync(new[] { Sample() });
            var result = await _controller.Search("phone");
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task Create_ValidDto_ReturnsCreated()
        {
            var dto = new CreateProductDto { Name = "New", Price = 100, DiscountPrice = 90, SKU = "N1", CategoryId = 1, ProductType = ProductType.Mobile };
            _mockService.Setup(s => s.CreateProductAsync(dto)).ReturnsAsync(Sample());
            var result = await _controller.Create(dto);
            Assert.IsType<CreatedAtActionResult>(result.Result);
        }

        [Fact]
        public async Task Update_Existing_ReturnsOk()
        {
            var dto = new UpdateProductDto { Name = "Updated", Price = 200, DiscountPrice = 180 };
            _mockService.Setup(s => s.UpdateProductAsync(1, dto)).ReturnsAsync(Sample());
            var result = await _controller.Update(1, dto);
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task Update_NonExisting_ReturnsNotFound()
        {
            var dto = new UpdateProductDto { Name = "X", Price = 1, DiscountPrice = 1 };
            _mockService.Setup(s => s.UpdateProductAsync(999, dto)).ReturnsAsync((ProductDto?)null);
            var result = await _controller.Update(999, dto);
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Delete_Existing_ReturnsNoContent()
        {
            _mockService.Setup(s => s.DeleteProductAsync(1)).ReturnsAsync(true);
            var result = await _controller.Delete(1);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_NonExisting_ReturnsNotFound()
        {
            _mockService.Setup(s => s.DeleteProductAsync(999)).ReturnsAsync(false);
            var result = await _controller.Delete(999);
            Assert.IsType<NotFoundResult>(result);
        }
    }

    public class CategoriesControllerTests
    {
        private readonly Mock<ICategoryService> _mockService;
        private readonly CategoriesController _controller;

        public CategoriesControllerTests()
        {
            _mockService = new Mock<ICategoryService>();
            _controller = new CategoriesController(_mockService.Object);
        }

        private static CategoryDto Cat(int id = 1) => new() { Id = id, Name = $"Cat{id}", ProductType = ProductType.Mobile };

        [Fact]
        public async Task GetAll_ReturnsOk()
        {
            _mockService.Setup(s => s.GetAllCategoriesAsync()).ReturnsAsync(new[] { Cat(1), Cat(2) });
            var result = await _controller.GetAll();
            var ok = Assert.IsType<OkObjectResult>(result.Result);
            Assert.IsAssignableFrom<IEnumerable<CategoryDto>>(ok.Value);
        }

        [Fact]
        public async Task GetById_Existing_ReturnsOk()
        {
            _mockService.Setup(s => s.GetCategoryByIdAsync(1)).ReturnsAsync(Cat());
            var result = await _controller.GetById(1);
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetById_NonExisting_ReturnsNotFound()
        {
            _mockService.Setup(s => s.GetCategoryByIdAsync(999)).ReturnsAsync((CategoryDto?)null);
            var result = await _controller.GetById(999);
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetMobileCategories_ReturnsOk()
        {
            _mockService.Setup(s => s.GetCategoriesByTypeAsync(ProductType.Mobile)).ReturnsAsync(new[] { Cat() });
            var result = await _controller.GetMobileCategories();
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetGroceryCategories_ReturnsOk()
        {
            _mockService.Setup(s => s.GetCategoriesByTypeAsync(ProductType.Grocery))
                .ReturnsAsync(new[] { new CategoryDto { Id = 5, Name = "Fruits", ProductType = ProductType.Grocery } });
            var result = await _controller.GetGroceryCategories();
            Assert.IsType<OkObjectResult>(result.Result);
        }
    }

    public class CartControllerTests
    {
        private readonly Mock<ICartService> _mockService;
        private readonly CartController _controller;

        public CartControllerTests()
        {
            _mockService = new Mock<ICartService>();
            _controller = new CartController(_mockService.Object);
        }

        private static CartItemDto CartItem(int id = 1) => new() { Id = id, ProductId = 1, ProductName = "Test", Quantity = 2, ProductPrice = 100 };

        [Fact]
        public async Task GetCart_ReturnsOk()
        {
            _mockService.Setup(s => s.GetCartAsync("sess")).ReturnsAsync(new[] { CartItem() });
            var result = await _controller.GetCart("sess");
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task AddToCart_ValidProduct_ReturnsOk()
        {
            var dto = new AddToCartDto { ProductId = 1, Quantity = 1 };
            _mockService.Setup(s => s.AddToCartAsync("sess", dto)).ReturnsAsync(CartItem());
            var result = await _controller.AddToCart("sess", dto);
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task AddToCart_InvalidProduct_ReturnsBadRequest()
        {
            var dto = new AddToCartDto { ProductId = 999, Quantity = 1 };
            _mockService.Setup(s => s.AddToCartAsync("sess", dto)).ThrowsAsync(new ArgumentException("Product not found"));
            var result = await _controller.AddToCart("sess", dto);
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task UpdateCartItem_Existing_ReturnsOk()
        {
            var dto = new UpdateCartDto { Quantity = 5 };
            _mockService.Setup(s => s.UpdateCartItemAsync(1, dto)).ReturnsAsync(CartItem());
            var result = await _controller.UpdateCartItem(1, dto);
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task UpdateCartItem_NonExisting_ReturnsNotFound()
        {
            var dto = new UpdateCartDto { Quantity = 5 };
            _mockService.Setup(s => s.UpdateCartItemAsync(999, dto)).ReturnsAsync((CartItemDto?)null);
            var result = await _controller.UpdateCartItem(999, dto);
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task RemoveFromCart_Existing_ReturnsNoContent()
        {
            _mockService.Setup(s => s.RemoveFromCartAsync(1)).ReturnsAsync(true);
            var result = await _controller.RemoveFromCart(1);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task RemoveFromCart_NonExisting_ReturnsNotFound()
        {
            _mockService.Setup(s => s.RemoveFromCartAsync(999)).ReturnsAsync(false);
            var result = await _controller.RemoveFromCart(999);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task ClearCart_ReturnsNoContent()
        {
            _mockService.Setup(s => s.ClearCartAsync("sess")).ReturnsAsync(true);
            var result = await _controller.ClearCart("sess");
            Assert.IsType<NoContentResult>(result);
        }
    }

    public class OrdersControllerTests
    {
        private readonly Mock<IOrderService> _mockService;
        private readonly OrdersController _controller;

        public OrdersControllerTests()
        {
            _mockService = new Mock<IOrderService>();
            _controller = new OrdersController(_mockService.Object);
        }

        private static OrderDto Order(int id = 1) => new()
        {
            Id = id, OrderNumber = $"FK-00{id}", CustomerName = "Test", TotalAmount = 500,
            Status = Core.Enums.OrderStatus.Pending, Items = new()
        };

        [Fact]
        public async Task GetAll_ReturnsOk()
        {
            _mockService.Setup(s => s.GetAllOrdersAsync()).ReturnsAsync(new[] { Order(1) });
            var result = await _controller.GetAll();
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetById_Existing_ReturnsOk()
        {
            _mockService.Setup(s => s.GetOrderByIdAsync(1)).ReturnsAsync(Order());
            var result = await _controller.GetById(1);
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetById_NonExisting_ReturnsNotFound()
        {
            _mockService.Setup(s => s.GetOrderByIdAsync(999)).ReturnsAsync((OrderDto?)null);
            var result = await _controller.GetById(999);
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Create_ValidCart_ReturnsCreated()
        {
            var dto = new CreateOrderDto { SessionId = "sess", CustomerName = "John", CustomerEmail = "j@j.com", ShippingAddress = "123 St" };
            _mockService.Setup(s => s.CreateOrderAsync(dto)).ReturnsAsync(Order());
            var result = await _controller.Create(dto);
            Assert.IsType<CreatedAtActionResult>(result.Result);
        }

        [Fact]
        public async Task Create_EmptyCart_ReturnsBadRequest()
        {
            var dto = new CreateOrderDto { SessionId = "empty" };
            _mockService.Setup(s => s.CreateOrderAsync(dto)).ThrowsAsync(new InvalidOperationException("Cart is empty"));
            var result = await _controller.Create(dto);
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task UpdateStatus_Existing_ReturnsOk()
        {
            _mockService.Setup(s => s.UpdateOrderStatusAsync(1, Core.Enums.OrderStatus.Shipped)).ReturnsAsync(Order());
            var result = await _controller.UpdateStatus(1, Core.Enums.OrderStatus.Shipped);
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task UpdateStatus_NonExisting_ReturnsNotFound()
        {
            _mockService.Setup(s => s.UpdateOrderStatusAsync(999, Core.Enums.OrderStatus.Shipped)).ReturnsAsync((OrderDto?)null);
            var result = await _controller.UpdateStatus(999, Core.Enums.OrderStatus.Shipped);
            Assert.IsType<NotFoundResult>(result.Result);
        }
    }

    public class HealthControllerTests
    {
        private readonly HealthController _controller = new();

        [Fact]
        public void Get_ReturnsOkWithHealthStatus()
        {
            var result = _controller.Get();
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void Ready_ReturnsOk()
        {
            var result = _controller.Ready();
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void Live_ReturnsOk()
        {
            var result = _controller.Live();
            Assert.IsType<OkObjectResult>(result);
        }
    }
}

using FlipkartApp.Core.DTOs;
using FlipkartApp.Core.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace FlipkartApp.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("FlipkartAPI");

            try
            {
                var mobileResponse = await client.GetStringAsync("/api/products/mobile");
                var groceryResponse = await client.GetStringAsync("/api/products/grocery");

                ViewBag.MobileProducts = JsonSerializer.Deserialize<List<ProductDto>>(mobileResponse, _jsonOptions) ?? new();
                ViewBag.GroceryProducts = JsonSerializer.Deserialize<List<ProductDto>>(groceryResponse, _jsonOptions) ?? new();
            }
            catch
            {
                ViewBag.MobileProducts = new List<ProductDto>();
                ViewBag.GroceryProducts = new List<ProductDto>();
            }

            return View();
        }

        public async Task<IActionResult> Mobile()
        {
            var client = _httpClientFactory.CreateClient("FlipkartAPI");
            try
            {
                var response = await client.GetStringAsync("/api/products/mobile");
                var products = JsonSerializer.Deserialize<List<ProductDto>>(response, _jsonOptions) ?? new();

                var catResponse = await client.GetStringAsync("/api/categories/mobile");
                var categories = JsonSerializer.Deserialize<List<CategoryDto>>(catResponse, _jsonOptions) ?? new();

                ViewBag.Categories = categories;
                return View(products);
            }
            catch
            {
                ViewBag.Categories = new List<CategoryDto>();
                return View(new List<ProductDto>());
            }
        }

        public async Task<IActionResult> Grocery()
        {
            var client = _httpClientFactory.CreateClient("FlipkartAPI");
            try
            {
                var response = await client.GetStringAsync("/api/products/grocery");
                var products = JsonSerializer.Deserialize<List<ProductDto>>(response, _jsonOptions) ?? new();

                var catResponse = await client.GetStringAsync("/api/categories/grocery");
                var categories = JsonSerializer.Deserialize<List<CategoryDto>>(catResponse, _jsonOptions) ?? new();

                ViewBag.Categories = categories;
                return View(products);
            }
            catch
            {
                ViewBag.Categories = new List<CategoryDto>();
                return View(new List<ProductDto>());
            }
        }

        public async Task<IActionResult> ProductDetail(int id)
        {
            var client = _httpClientFactory.CreateClient("FlipkartAPI");
            try
            {
                var response = await client.GetStringAsync($"/api/products/{id}");
                var product = JsonSerializer.Deserialize<ProductDto>(response, _jsonOptions);
                if (product == null) return NotFound();
                return View(product);
            }
            catch
            {
                return NotFound();
            }
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}

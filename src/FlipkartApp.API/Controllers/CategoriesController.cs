using FlipkartApp.API.Services;
using FlipkartApp.Core.DTOs;
using FlipkartApp.Core.Enums;
using Microsoft.AspNetCore.Mvc;

namespace FlipkartApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAll()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetById(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null) return NotFound();
            return Ok(category);
        }

        [HttpGet("mobile")]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetMobileCategories()
        {
            var categories = await _categoryService.GetCategoriesByTypeAsync(ProductType.Mobile);
            return Ok(categories);
        }

        [HttpGet("grocery")]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetGroceryCategories()
        {
            var categories = await _categoryService.GetCategoriesByTypeAsync(ProductType.Grocery);
            return Ok(categories);
        }
    }
}

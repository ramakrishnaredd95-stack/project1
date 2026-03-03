using FlipkartApp.API.Services;
using FlipkartApp.Core.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace FlipkartApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("{sessionId}")]
        public async Task<ActionResult<IEnumerable<CartItemDto>>> GetCart(string sessionId)
        {
            var items = await _cartService.GetCartAsync(sessionId);
            return Ok(items);
        }

        [HttpPost("{sessionId}")]
        public async Task<ActionResult<CartItemDto>> AddToCart(string sessionId, [FromBody] AddToCartDto dto)
        {
            try
            {
                var item = await _cartService.AddToCartAsync(sessionId, dto);
                return Ok(item);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CartItemDto>> UpdateCartItem(int id, [FromBody] UpdateCartDto dto)
        {
            var item = await _cartService.UpdateCartItemAsync(id, dto);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveFromCart(int id)
        {
            var result = await _cartService.RemoveFromCartAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("clear/{sessionId}")]
        public async Task<IActionResult> ClearCart(string sessionId)
        {
            await _cartService.ClearCartAsync(sessionId);
            return NoContent();
        }
    }
}

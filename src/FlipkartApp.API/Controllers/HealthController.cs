using Microsoft.AspNetCore.Mvc;

namespace FlipkartApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                Status = "Healthy",
                Timestamp = DateTime.UtcNow,
                Service = "FlipkartApp.API",
                Version = "1.0.0"
            });
        }

        [HttpGet("ready")]
        public IActionResult Ready()
        {
            return Ok(new { Status = "Ready" });
        }

        [HttpGet("live")]
        public IActionResult Live()
        {
            return Ok(new { Status = "Live" });
        }
    }
}

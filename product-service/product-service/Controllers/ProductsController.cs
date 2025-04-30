using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace product_service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        // ✅ This API REQUIRES a valid JWT token
        [HttpGet("secure")]
        [Authorize]
        public IActionResult GetSecureProducts()
        {
            return Ok(new[] {
                new { Id = 1, Name = "Product A (secured)" },
                new { Id = 2, Name = "Product B (secured)" }
            });
        }

        // ✅ This API is PUBLIC - no JWT required
        [HttpGet("public")]
        [AllowAnonymous]
        public IActionResult GetPublicProducts()
        {
            return Ok(new[] {
                new { Id = 101, Name = "Product X (public)" },
                new { Id = 102, Name = "Product Y (public)" }
            });
        }
    }
}

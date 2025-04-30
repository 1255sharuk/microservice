using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace order_service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        // ✅ Requires JWT token
        [HttpGet("secure")]
        [Authorize]
        public IActionResult GetSecureOrders()
        {
            return Ok(new[] {
                new { OrderId = 1, Order = "Order A", Quantity = 2 },
                new { OrderId = 2, Order = "Order B", Quantity = 1 }
            });
        }

        // ✅ Public API (no token needed)
        [HttpGet("public")]
        [AllowAnonymous]
        public IActionResult GetPublicOrders()
        {
            return Ok(new[] {
                new { OrderId = 101, Order = "Order X", Quantity = 10 },
                new { OrderId = 102, Order = "Order Y", Quantity = 5 }
            });
        }
    }
}

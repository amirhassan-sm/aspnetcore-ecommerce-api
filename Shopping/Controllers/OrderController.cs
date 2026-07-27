using Application.Contrast.Services;
using Application.DTOs.Basket;
using Application.DTOs.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shopping.Security;
using System.Security.Claims;

namespace Shopping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderManagament orderService;

        public OrderController(IOrderManagament orderService)
        {
            this.orderService = orderService;
        }

        [AllowAnonymous]
        [HttpPost("basket")]
        public async Task<IActionResult> GetBasket([FromBody] List<BasketItemDto> items)
        {
            var result = await orderService.GetBasketList(items);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("check-quantity")]
        public async Task<IActionResult> CheckQuantity([FromQuery] int productId, [FromQuery] int quantity)
        {
            var result = await orderService.IsProductQuantityAvailabe(productId, quantity);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterFinalOrder([FromBody] OrderDto order)
        {
            if (!TryGetUserId(out int userId))
                return Unauthorized();

            order.UserId = userId;

            var result = await orderService.RegisterFinalOrder(order);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize]
        [HttpGet("{orderId:int}")]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            if (!TryGetUserId(out int userId))
                return Unauthorized();

            var isAdmin = User.IsInRole(AppRoles.Admin);
            var result = await orderService.GetOrderById(orderId, userId, isAdmin);
            if (!result.Success)
                return result.ErrorCode == "Forbidden" ? Forbid() : BadRequest(result);

            return Ok(result);
        }

        [Authorize]
        [HttpGet("my-orders")]
        public async Task<IActionResult> GetMyOrders()
        {
            if (!TryGetUserId(out int userId))
                return Unauthorized();

            var result = await orderService.GetOrdersByUserId(userId);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize]
        [HttpPost("{orderId:int}/cancel")]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            if (!TryGetUserId(out int userId))
                return Unauthorized();

            var isAdmin = User.IsInRole(AppRoles.Admin);
            var result = await orderService.CancelOrder(orderId, userId, isAdmin);
            if (!result.Success)
                return result.ErrorCode == "Forbidden" ? Forbid() : BadRequest(result);

            return Ok(result);
        }

        private bool TryGetUserId(out int userId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdClaim, out userId);
        }
    }
}

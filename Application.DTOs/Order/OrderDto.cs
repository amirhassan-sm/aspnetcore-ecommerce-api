using Application.DTOs.Basket;

namespace Application.DTOs.Order
{
    public class OrderDto
    {
        public int? UserId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Province { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string? Street { get; set; }
        public List<BasketItemDto> basketItem { get; set; } = new();
    }
}

using Application.DTOs.Basket;

namespace Application.DTOs.Order
{
    public class OrderResultDto
    {
        public int OrderId { get; set; }
        public int? UserId { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public BasketSummeryDto Summery { get; set; } = new();
        public List<BasketListItemDto> Items { get; set; } = new();
    }
}

namespace Application.DTOs.Basket
{
    /// <summary>
    /// آیتم غنی‌شده سبد برای نمایش (نام، قیمت، موجودی، تصویر).
    /// </summary>
    public class BasketListItemDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal LineTotal { get; set; }
        public int AvailableProductCount { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsAvailable { get; set; }
    }
}

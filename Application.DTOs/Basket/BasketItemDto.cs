namespace Application.DTOs.Basket
{
    /// <summary>
    /// آیتم خام سبد که از کلاینت می‌آید (فقط شناسه و تعداد).
    /// </summary>
    public class BasketItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}

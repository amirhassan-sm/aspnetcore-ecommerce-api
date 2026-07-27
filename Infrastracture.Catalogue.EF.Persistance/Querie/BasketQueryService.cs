using Application.Contrast.QueryServices;
using Application.DTOs.Basket;
using Microsoft.EntityFrameworkCore;

namespace Infrastracture.Catalogue.EF.Persistance.Querie
{
    public class BasketQueryService : IBasketQueryService
    {
        private readonly CatalogueContext db;

        public BasketQueryService(CatalogueContext db)
        {
            this.db = db;
        }

        public async Task<BasketResultDto> GetBasketAsync(List<BasketItemDto> basketItems)
        {
            var result = new BasketResultDto();

            if (basketItems == null || basketItems.Count == 0)
                return result;

            // اگر یک محصول چند بار آمده، تعدادها را جمع می‌کنیم
            var requested = basketItems
                .Where(x => x.ProductId > 0 && x.Quantity > 0)
                .GroupBy(x => x.ProductId)
                .Select(g => new { ProductId = g.Key, Quantity = g.Sum(x => x.Quantity) })
                .ToList();

            if (requested.Count == 0)
                return result;

            var productIds = requested.Select(x => x.ProductId).ToList();

            // محصولات + اولین تصویر تأییدشده (DisplayOrder کمینه)
            var products = await (
                from p in db.products.AsNoTracking()
                where productIds.Contains(p.ProductId)
                select new
                {
                    p.ProductId,
                    p.ProductName,
                    p.BasePrice,
                    p.AvailableProductCount,
                    ImageUrl = db.productMedias
                        .Where(m => m.ProductId == p.ProductId && m.IsConfirmed)
                        .OrderBy(m => m.DisplayOrder)
                        .Select(m => m.Url)
                        .FirstOrDefault()
                }
            ).ToListAsync();

            var productMap = products.ToDictionary(x => x.ProductId);

            foreach (var item in requested)
            {
                if (!productMap.TryGetValue(item.ProductId, out var product))
                    continue; // محصول حذف‌شده / ناموجود در کاتالوگ

                var lineTotal = product.BasePrice * item.Quantity;

                result.basketListItems.Add(new BasketListItemDto
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    UnitPrice = product.BasePrice,
                    Quantity = item.Quantity,
                    LineTotal = lineTotal,
                    AvailableProductCount = product.AvailableProductCount,
                    ImageUrl = product.ImageUrl,
                    IsAvailable = product.AvailableProductCount >= item.Quantity
                });
            }

            result.Summery.TotalPrice = result.basketListItems.Sum(x => x.LineTotal);
            result.Summery.totalProductQuantity = result.basketListItems.Sum(x => x.Quantity);

            return result;
        }
    }
}

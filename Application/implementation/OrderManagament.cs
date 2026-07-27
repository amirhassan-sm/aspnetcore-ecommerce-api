using Application.Common.BaseModel;
using Application.Contrast.QueryServices;
using Application.Contrast.Services;
using Application.DTOs.Basket;
using Application.DTOs.Order;
using Catalogue.DomainServiceContract.Service;
using Sale.Domain.Models;
using Sale.DomainServiceContract.Services;

namespace Application.implementation
{
    public class OrderManagament : IOrderManagament
    {
        private const string StatusConfirmed = "Confirmed";
        private const string StatusCancelled = "Cancelled";

        private readonly IProductRepository productRepo;
        private readonly IBasketQueryService basketService;
        private readonly IOrderRepository orderRepo;

        public OrderManagament(
            IProductRepository productRepo,
            IBasketQueryService basketService,
            IOrderRepository orderRepo)
        {
            this.productRepo = productRepo;
            this.basketService = basketService;
            this.orderRepo = orderRepo;
        }

        public async Task<GenericOperationResult<BasketResultDto>> GetBasketList(List<BasketItemDto> BasketProductList)
        {
            try
            {
                var basket = await basketService.GetBasketAsync(BasketProductList ?? new List<BasketItemDto>());
                return GenericOperationResult<BasketResultDto>.ToSuccess("basket loaded", basket);
            }
            catch (Exception)
            {
                return GenericOperationResult<BasketResultDto>.ToFail(
                    "failed to get basket",
                    new List<string> { "An unexpected error occurred." },
                    "Exception_Occured");
            }
        }

        public async Task<OperationResult> IsProductQuantityAvailabe(int productId, int quantity)
        {
            try
            {
                if (quantity <= 0)
                    return OperationResult.ToFail("invalid quantity", new List<string> { "Quantity must be greater than zero." });

                var product = await productRepo.Get(productId);
                if (product == null)
                    return OperationResult.ToFail(productId, "product not found", new List<string> { "Product does not exist." });

                if (product.AvailableProductCount < quantity)
                    return OperationResult.ToFail(productId, "not enough stock", new List<string>
                    {
                        $"Available: {product.AvailableProductCount}, requested: {quantity}"
                    });

                return OperationResult.ToSuccess(productId, "quantity is available");
            }
            catch (Exception)
            {
                return OperationResult.ToFail("check failed", new List<string> { "An unexpected error occurred." }, "Exception_Occured");
            }
        }

        public async Task<GenericOperationResult<OrderResultDto>> RegisterFinalOrder(OrderDto order)
        {
            var deductedItems = new List<(int ProductId, int Quantity)>();

            try
            {
                if (order == null || order.basketItem == null || order.basketItem.Count == 0)
                    return GenericOperationResult<OrderResultDto>.ToFail(
                        "invalid order",
                        new List<string> { "Order must contain at least one basket item." });

                if (string.IsNullOrWhiteSpace(order.CustomerName) ||
                    string.IsNullOrWhiteSpace(order.Phone) ||
                    string.IsNullOrWhiteSpace(order.Province))
                {
                    return GenericOperationResult<OrderResultDto>.ToFail(
                        "invalid order",
                        new List<string> { "CustomerName, Phone and Province are required." });
                }

                // قیمت و موجودی فقط از سرور
                var basket = await basketService.GetBasketAsync(order.basketItem);
                if (basket.basketListItems.Count == 0)
                    return GenericOperationResult<OrderResultDto>.ToFail(
                        "empty basket",
                        new List<string> { "No valid products found for this basket." });

                var unavailable = basket.basketListItems.Where(x => !x.IsAvailable).ToList();
                if (unavailable.Count > 0)
                {
                    return GenericOperationResult<OrderResultDto>.ToFail(
                        "not enough stock",
                        unavailable.Select(x =>
                            $"Product {x.ProductId} ({x.ProductName}): available {x.AvailableProductCount}, requested {x.Quantity}"
                        ).ToList());
                }

                // کسر موجودی
                foreach (var item in basket.basketListItems)
                {
                    await productRepo.DecreseQuantity(item.ProductId, item.Quantity);
                    deductedItems.Add((item.ProductId, item.Quantity));
                }

                var saleOrder = new Order
                {
                    UserId = order.UserId,
                    CustomerName = order.CustomerName.Trim(),
                    Phone = order.Phone.Trim(),
                    Province = order.Province.Trim(),
                    City = order.City?.Trim() ?? string.Empty,
                    Street = order.Street,
                    Orderdate = DateTime.UtcNow,
                    TotalAmount = basket.Summery.TotalPrice,
                    Status = StatusConfirmed
                };

                foreach (var item in basket.basketListItems)
                {
                    saleOrder.orderDetails.Add(new OrderDetails
                    {
                        ProductId = item.ProductId,
                        ProductName = item.ProductName,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    });
                }

                var saved = await orderRepo.RegisterOrder(saleOrder);

                var result = new OrderResultDto
                {
                    OrderId = saved.OrderId,
                    UserId = saved.UserId,
                    Status = saved.Status,
                    OrderDate = saved.Orderdate,
                    Summery = basket.Summery,
                    Items = basket.basketListItems
                };

                return GenericOperationResult<OrderResultDto>.ToSuccess(saved.OrderId, "order registered", result);
            }
            catch (Exception)
            {
                // اگر ثبت سفارش fail شد، موجودی برگشت داده شود
                foreach (var item in deductedItems)
                {
                    try
                    {
                        await productRepo.IncreaseQuantity(item.ProductId, item.Quantity);
                    }
                    catch
                    {
                        // best-effort rollback
                    }
                }

                return GenericOperationResult<OrderResultDto>.ToFail(
                    "register failed",
                    new List<string> { "An unexpected error occurred." },
                    "Exception_Occured");
            }
        }

        public async Task<GenericOperationResult<OrderResultDto>> GetOrderById(int orderId, int requestingUserId, bool isAdmin)
        {
            try
            {
                var order = await orderRepo.GetById(orderId);
                if (order == null)
                    return GenericOperationResult<OrderResultDto>.ToFail(
                        orderId,
                        "order not found",
                        new List<string> { "Order does not exist." });

                if (!isAdmin && order.UserId != requestingUserId)
                    return GenericOperationResult<OrderResultDto>.ToFail(
                        orderId,
                        "forbidden",
                        new List<string> { "You can only view your own orders." },
                        "Forbidden");

                var result = MapToOrderResult(order);
                return GenericOperationResult<OrderResultDto>.ToSuccess(orderId, "order loaded", result);
            }
            catch (Exception)
            {
                return GenericOperationResult<OrderResultDto>.ToFail(
                    "get failed",
                    new List<string> { "An unexpected error occurred." },
                    "Exception_Occured");
            }
        }

        public async Task<GenericOperationResult<List<OrderListItemDto>>> GetOrdersByUserId(int userId)
        {
            try
            {
                var orders = await orderRepo.GetByUserId(userId);
                var list = orders.Select(o => new OrderListItemDto
                {
                    OrderId = o.OrderId,
                    CustomerName = o.CustomerName,
                    Status = o.Status,
                    OrderDate = o.Orderdate,
                    TotalAmount = o.TotalAmount
                }).ToList();

                return GenericOperationResult<List<OrderListItemDto>>.ToSuccess("orders list", list);
            }
            catch (Exception)
            {
                return GenericOperationResult<List<OrderListItemDto>>.ToFail(
                    "get failed",
                    new List<string> { "An unexpected error occurred." },
                    "Exception_Occured");
            }
        }

        public async Task<OperationResult> CancelOrder(int orderId, int requestingUserId, bool isAdmin)
        {
            try
            {
                var order = await orderRepo.GetById(orderId);
                if (order == null)
                    return OperationResult.ToFail(orderId, "order not found", new List<string> { "Order does not exist." });

                if (!isAdmin && order.UserId != requestingUserId)
                    return OperationResult.ToFail(orderId, "forbidden", new List<string> { "You can only cancel your own orders." }, "Forbidden");

                if (string.Equals(order.Status, StatusCancelled, StringComparison.OrdinalIgnoreCase))
                    return OperationResult.ToFail(orderId, "already cancelled", new List<string> { "Order is already cancelled." });

                foreach (var detail in order.orderDetails)
                {
                    await productRepo.IncreaseQuantity(detail.ProductId, detail.Quantity);
                }

                order.Status = StatusCancelled;
                await orderRepo.UpdateOrder(order);

                return OperationResult.ToSuccess(orderId, "order cancelled and stock restored");
            }
            catch (Exception)
            {
                return OperationResult.ToFail("cancel failed", new List<string> { "An unexpected error occurred." }, "Exception_Occured");
            }
        }

        private static OrderResultDto MapToOrderResult(Order order)
        {
            var items = order.orderDetails.Select(d => new BasketListItemDto
            {
                ProductId = d.ProductId,
                ProductName = d.ProductName,
                UnitPrice = d.UnitPrice,
                Quantity = d.Quantity,
                LineTotal = d.UnitPrice * d.Quantity,
                AvailableProductCount = 0,
                IsAvailable = true
            }).ToList();

            return new OrderResultDto
            {
                OrderId = order.OrderId,
                UserId = order.UserId,
                Status = order.Status,
                OrderDate = order.Orderdate,
                Summery = new BasketSummeryDto
                {
                    TotalPrice = order.TotalAmount,
                    totalProductQuantity = items.Sum(x => x.Quantity)
                },
                Items = items
            };
        }
    }
}

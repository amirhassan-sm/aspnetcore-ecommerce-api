using Application.Common.BaseModel;
using Application.DTOs.Basket;
using Application.DTOs.Order;

namespace Application.Contrast.Services
{
    public interface IOrderManagament
    {
        Task<OperationResult> IsProductQuantityAvailabe(int productId, int quantity);

        Task<GenericOperationResult<BasketResultDto>> GetBasketList(List<BasketItemDto> BasketProductList);

        Task<GenericOperationResult<OrderResultDto>> RegisterFinalOrder(OrderDto order);

        Task<GenericOperationResult<OrderResultDto>> GetOrderById(int orderId, int requestingUserId, bool isAdmin);

        Task<GenericOperationResult<List<OrderListItemDto>>> GetOrdersByUserId(int userId);

        Task<OperationResult> CancelOrder(int orderId, int requestingUserId, bool isAdmin);
    }
}

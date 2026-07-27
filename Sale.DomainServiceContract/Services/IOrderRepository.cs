using Sale.Domain.Models;

namespace Sale.DomainServiceContract.Services
{
    public interface IOrderRepository
    {
        Task<bool> HasOrder(int productId);
        Task<Order> RegisterOrder(Order order);
        Task<Order?> GetById(int orderId);
        Task<List<Order>> GetByUserId(int userId);
        Task UpdateOrder(Order order);
    }
}

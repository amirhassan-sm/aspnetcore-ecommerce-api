using Microsoft.EntityFrameworkCore;
using Sale.Domain.Models;
using Sale.DomainServiceContract.Services;

namespace Infrastructure.Sale.repositry
{
    public class OrderRepository : IOrderRepository
    {
        private readonly SaleSystemContext db;

        public OrderRepository(SaleSystemContext db)
        {
            this.db = db;
        }

        public async Task<bool> HasOrder(int productId)
        {
            return await db.OrderDetail.AnyAsync(x => x.ProductId == productId);
        }

        public async Task<Order> RegisterOrder(Order order)
        {
            db.Orders.Add(order);
            await db.SaveChangesAsync();
            return order;
        }

        public async Task<Order?> GetById(int orderId)
        {
            return await db.Orders
                .Include(x => x.orderDetails)
                .FirstOrDefaultAsync(x => x.OrderId == orderId);
        }

        public async Task<List<Order>> GetByUserId(int userId)
        {
            return await db.Orders
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.Orderdate)
                .ToListAsync();
        }

        public async Task UpdateOrder(Order order)
        {
            db.Orders.Update(order);
            await db.SaveChangesAsync();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sale.Domain.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int? UserId { get; set; }

        public string CustomerName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Province { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;
        public string? Street { get; set; }
        public DateTime Orderdate { get; set; }
        public decimal TotalAmount { get; set; }

        public string Status { get; set; } = string.Empty;

        public List<OrderDetails> orderDetails { get; set; }

        public Order()
        {
            orderDetails = new List<OrderDetails>();
        }



    }
}

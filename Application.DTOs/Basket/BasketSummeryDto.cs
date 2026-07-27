using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Basket
{
    public class BasketSummeryDto
    {
        public decimal TotalPrice { get; set; }
        //public decimal totalDiscount { get; set; } later

        public int totalProductQuantity { get; set; }
    }
}

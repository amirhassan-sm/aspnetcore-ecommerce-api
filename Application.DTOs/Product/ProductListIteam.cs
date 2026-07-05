using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Product
{
    public class ProductListIteam
    {
        public int ProducId { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string ProductDescription { get; set; } = string.Empty;

        public decimal UnitePrice{ get; set; }

        public int availbleCount { get; set; }
        public string SuplierName { get; set; } = string.Empty;



    }
}

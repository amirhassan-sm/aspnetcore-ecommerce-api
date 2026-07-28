using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Product
{
    public class ProductAddModel
    {
        
        public string ProductName { get; set; }

        public string ProductDescription { get; set; }
       
        public int CategoryId { get; set; }
        
        public decimal BasePrice { get; set; }

        public string Introduction { get; set; }
     
        public int AvailableProductCount { get; set; }
    }
}

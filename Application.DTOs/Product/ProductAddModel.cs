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
        [Required]
        public string ProductName { get; set; }

        public string ProductDescription { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public decimal BasePrice { get; set; }

        public string ImageUrl { get; set; }

        public string Introduction { get; set; }
        [Required]
        public int AvailableProductCount { get; set; }
    }
}

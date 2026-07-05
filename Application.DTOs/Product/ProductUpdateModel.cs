using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Product
{
    public class ProductUpdateModel
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public string ProductName { get; set; }

        public string ProductDescription { get; set; }

        public decimal BasePrice { get; set; }

        public string ImageUrl { get; set; }

        public string Introduction { get; set; }
        public string ExtraData { get; set; }
        public string BodyExtraData { get; set; }

        public string Slug { get; set; }
        public string PageTitle { get; set; }
        public string MetaTag { get; set; }
        public string MetaDescription { get; set; }
        public int AvailableProductCount { get; set; }
    }
}

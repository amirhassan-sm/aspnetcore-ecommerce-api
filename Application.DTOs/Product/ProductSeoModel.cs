using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Product
{
    public class ProductSeoModel
    {
        public int ProductId { get; set; }


        public string ExtraData { get; set; }= string.Empty;
        public string BodyExtraData { get; set; }=string.Empty;

        public string Slug { get; set; } = string.Empty;
        public string PageTitle { get; set; } = string.Empty;
        public string MetaTag { get; set; } = string.Empty;
        public string MetaDescription { get; set; } = string.Empty;
    }
}

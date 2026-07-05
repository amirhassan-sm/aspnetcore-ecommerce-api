using Application.Common.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Product
{
    public class ProductSearchModel:PageModel
    {
        public string? Phrase { get; set; } 
        public int? ParentId { get; set; }
        public decimal? FromUnitePrice { get; set; }
        public decimal? ToUnitePrice { get; set; }

        public int? Level1CategoryId { get; set; }
        public int? Level2CategoryId { get; set; }
        public int? Level3CategoryId { get; set; }

    }
}

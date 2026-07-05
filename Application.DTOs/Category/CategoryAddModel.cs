using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Category
{
    public class CategoryAddModel
    {
        public string CategoryName { get; set; } = string.Empty;
        public string CategoryDescription { get; set; }=string.Empty;
        public string Slug { get; set; } = string.Empty;

        public int? ParentId { get; set; }
    }
}

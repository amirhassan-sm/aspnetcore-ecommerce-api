using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Category
{
    public class CategoryAddModel
    {
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }

        public int? ParentId { get; set; }
    }
}

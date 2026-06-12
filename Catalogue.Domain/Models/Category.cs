using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Domain.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string  CategoryDescription { get; set; }

        public int? ParentId { get; set; } 

        public string LineAge { get; set; }
        public string Slug { get; set; }
        public string MetaData{ get; set; }
        public string MetaTag { get; set; }

        public int? Depth { get; set; }

        public Category? Parent { get; set; }
        public List<Category> Children { get; set; } = new();
        public List<Product> Products { get; set; } = new();
        public List<CategoryFeature> CategoryFeatures    { get; set; } = new();
        


    }
}

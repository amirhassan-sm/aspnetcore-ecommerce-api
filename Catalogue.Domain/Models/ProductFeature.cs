using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Domain.Models
{
    public class ProductFeature
    {
        public int ProductFeatureId { get; set; }
        public int FeatureId { get; set; }
        public int ProductId { get; set; }
        public string Value { get; set; } = string.Empty;


        public Product product { get; set; }

        public Feature feature { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Domain.Models
{
    public class CategoryFeature
    {
        public int CategoryFeatureId { get; set; }
        public int CategoryId { get; set; }
        public int FeatureId { get; set; }

        public Category category { get; set; }
        public  Feature feature { get; set; }
    }
}

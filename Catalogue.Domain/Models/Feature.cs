using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Domain.Models
{
    public class Feature
    {
        public int FeatureId { get; set; }
        public string FeatureName { get; set; } 

        public string FeatureDescription { get; set; }

        public List<CategoryFeature> categoryFeatures { get; set; } = new List<CategoryFeature>();

        public List<ProductFeature> productFeatures { get; set; } = new List<ProductFeature>();


    }
}

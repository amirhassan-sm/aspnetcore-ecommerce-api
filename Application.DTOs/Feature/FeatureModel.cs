using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Feature
{
    public class FeatureModel
    {
        public int FeatureId { get; set; }
        public string FeatureName { get; set; } = string.Empty;

        public string FeatureDescription { get; set; } = string.Empty;
    }
}

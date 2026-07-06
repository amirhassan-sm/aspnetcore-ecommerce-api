using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Feature
{
    public class FeatureListItem
    {
        public int FeatureId { get; set; }
        public string FeatureName{ get; set; } =string.Empty;
        public string  Description { get; set; }=   string.Empty;
    }
}

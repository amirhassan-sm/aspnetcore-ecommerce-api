using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.ProductFeatures
{
    public class SetProductFeatureModel
    {
        public int FeatureId { get; set; }
        public int ProductId { get; set; }

        public string FeaureValue { get; set; } = string.Empty;
    }
}

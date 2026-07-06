using Application.Common.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Feature
{
    public class FeatureSearchModel:PageModel
    {
        public string? SearchPhrase { get; set; }
    }
}

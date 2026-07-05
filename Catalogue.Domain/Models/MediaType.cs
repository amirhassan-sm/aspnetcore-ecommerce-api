using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Domain.Models
{
    public class MediaType
    {
        public int MediaTypeId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public ICollection<ProductMedia> ProductMedia { get; set; }
            = new List<ProductMedia>();


    }
}

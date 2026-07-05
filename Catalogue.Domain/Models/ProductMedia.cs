using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Domain.Models
{
    public class ProductMedia
    {
        public int ProductMediaId { get; set; }
        public int ProductId { get; set; }

        public int MediaTypeId { get; set; }
        public string Url { get; set; } = string.Empty;
        public long fileSize { get; set; }
        public string  UploadingUserName { get; set; }=string.Empty;
        public bool  IsConfirmed { get; set; }
        public string  ConfirmByUserName { get; set; } = string.Empty;  

        public Product Product { get; set; }
        public MediaType MediaType{ get; set; }



    }

  
  
}

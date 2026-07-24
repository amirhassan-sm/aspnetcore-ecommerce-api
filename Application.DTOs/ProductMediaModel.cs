using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ProductMediaModel
    {
        public int ProductMediaId { get; set; }
        public int ProductId { get; set; }
        public int DisplayOrder { get; set; }

        public int MediaTypeId { get; set; }
        public string Url { get; set; } = string.Empty;
        public long fileSize { get; set; }
        public string UploadingUserName { get; set; } = string.Empty;
        public bool IsConfirmed { get; set; }
        public string ConfirmByUserName { get; set; } = string.Empty;
        public Stream FileStream { get; set; } = new MemoryStream();

        public string FileName { get; set; } = string.Empty;

    }
}

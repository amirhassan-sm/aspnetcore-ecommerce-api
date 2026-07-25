using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.ProducMedia
{
    public class ProductMediaEntryDto
    {
        public int ProductId { get; set; }
        public int MediaTypeId { get; set; }
        public bool IsConfirmed { get; set; }
        //public string ConfirmByUserName { get; set; } = string.Empty;
        public IFormFile FileStream { get; set; } 
    }
}

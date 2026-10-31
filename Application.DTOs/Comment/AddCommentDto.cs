using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Comment
{
    public class AddCommentDto
    {
        public int? ParentCommentId { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}

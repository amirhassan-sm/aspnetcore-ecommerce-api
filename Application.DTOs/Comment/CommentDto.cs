using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Comment
{
    public class CommentDto
    {
        public int CommentId { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public int? ParentCommentId { get; set; }
        public DateTime CommentDate { get; set; }
        public DateTime? UpdatedAt { get; set; }


        public string Content { get; set; } = string.Empty;
    }
}

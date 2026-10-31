using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Domain.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public int? ParentCommentId { get; set; }
        public DateTime CommentDate { get; set; }
        public DateTime? UpdatedAt { get; set; }


        public string Content { get; set; } = string.Empty;

       
        

        public Product Product { get; set; } = new();

        public List<Comment> Replies { get; set; } = new();

        public Comment? ParentComment { get; set; }

    }
}

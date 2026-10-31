using Application.Common.BaseModel;
using Application.DTOs.Comment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contrast.Services
{
    public interface ICommentService
    {
        Task<OperationResult> AddCommentAsync(AddCommentDto model,int productId,int UserId );
        Task<OperationResult> RemoveCommentAsync(int CommentId);
        Task<GenericOperationResult<CommentDto>> GetCommentByIdAsync(int CommentId);

        Task<OperationResult> UpdateCommentAsync(UpdateCommentDto model);

        Task<GenericOperationResult<List<CommentDto>>> GetCommentByProductIdAsync(int productId);



    }
}

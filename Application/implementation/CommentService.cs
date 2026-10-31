using Application.Common.BaseModel;
using Application.Contrast.Services;
using Application.DTOs.Comment;
using Catalogue.DomainServiceContract.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.implementation
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository repo;
        public CommentService(ICommentRepository repo)
        {
            this.repo = repo;
            
        }
        public Task<OperationResult> AddCommentAsync(AddCommentDto model, int productId, int UserId)
        {
            throw new NotImplementedException();
        }

        public Task<GenericOperationResult<CommentDto>> GetCommentByIdAsync(int CommentId)
        {
            throw new NotImplementedException();
        }

        public Task<GenericOperationResult<List<CommentDto>>> GetCommentByProductIdAsync(int productId)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> RemoveCommentAsync(int CommentId)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> UpdateCommentAsync(UpdateCommentDto model)
        {
            throw new NotImplementedException();
        }
    }
}

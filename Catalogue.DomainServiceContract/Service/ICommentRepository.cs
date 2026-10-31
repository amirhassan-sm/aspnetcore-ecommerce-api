using Catalogue.Domain.Models;
using Framwork.Domain.BaseInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.DomainServiceContract.Service
{
    public interface ICommentRepository: IBaseRepoModel<Comment,int>
    {

       Task<IReadOnlyList<Comment>> GetCommentsByProductIdAsync(int productId);

    }
}

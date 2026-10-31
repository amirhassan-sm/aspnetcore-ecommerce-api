
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framwork.Domain.BaseInterface
{
    public interface IBaseRepoModel<TModel,Tkey>
    {
        Task<TModel?> Get(Tkey id);
        Task<int> AddNew(TModel model);
        Task<bool> Remove(Tkey id);
        Task<bool> Update(TModel model);
      

    }
}

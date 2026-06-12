using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Security.DomainServiceContract.Services
{
    public interface IAthentication
    {
        public Task<bool> AuthenticateAsync();
    }
}

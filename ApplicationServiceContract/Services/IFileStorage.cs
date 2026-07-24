using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contrast.Services
{
    public interface IFileStorage
    {
        Task<string> SaveAsync(Stream fileStream, string fileName);
        Task DeleteAsync(string url);


    }
}

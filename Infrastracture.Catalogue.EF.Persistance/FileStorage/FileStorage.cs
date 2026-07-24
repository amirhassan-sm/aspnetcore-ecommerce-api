using Application.Contrast.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.Runtime.CompilerServices;

namespace Infrastracture.Catalogue.EF.Persistance.FileStorage
{
    public class FileStorage : IFileStorage
    {
        private readonly IWebHostEnvironment environment;
        public FileStorage(IWebHostEnvironment environment)
        {
            this.environment = environment;
        }
        public Task DeleteAsync(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return Task.CompletedTask;
            }

            var filePath = Path.Combine(
        environment.WebRootPath,
        url.TrimStart('/')
           .Replace('/', Path.DirectorySeparatorChar));

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            return Task.CompletedTask;




        }



        public async Task<string> SaveAsync(Stream fileStream, string fileName)
        {
            var folderPath = Path.Combine(environment.WebRootPath, "images", "products");


            if (Directory.Exists(folderPath))
            {

                Directory.CreateDirectory(folderPath);
            }
            var extention = Path.GetExtension(fileName);

            var newFileName = $"{Guid.NewGuid()}{extention}";

            var fullPath = Path.Combine(folderPath, newFileName);

            using (var output = new FileStream(fullPath, FileMode.Create))
            {
                await fileStream.CopyToAsync(output);
            }

            return $"/images/products/{newFileName}";

        }
    }
}

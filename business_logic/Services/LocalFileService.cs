using business_logic.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace business_logic.Services
{
    public class LocalFileService : IFileService
    {
        private const string imageFolder = "Images";
        private readonly IWebHostEnvironment environment;

        public LocalFileService(IWebHostEnvironment environment)
        {
            this.environment = environment;
        }

        public Task DeleteProductImage(string path)
        {
            throw new NotImplementedException();
        }

        public Task DeleteProductImageExcept(string[] files)
        {
            throw new NotImplementedException();
        }

        public async Task<string> SaveImage(IFormFile file)
        {
            string root = environment.WebRootPath;      // wwwroot
            string name = Guid.NewGuid().ToString();    // random name
            string extension = Path.GetExtension(file.FileName); // get original extension
            string fullName = name + extension;         // full name: name.ext

            // create destination image file path
            string imagePath = Path.Combine(imageFolder, fullName);
            string imageFullPath = Path.Combine(root, imagePath);

            // save image on the folder
            using (FileStream fs = new FileStream(imageFullPath, FileMode.Create))
            {
                await file.CopyToAsync(fs);
            }

            // return image file path
            return Path.DirectorySeparatorChar + imagePath;
        }
    }
}

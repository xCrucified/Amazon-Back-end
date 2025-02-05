using business_logic.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace business_logic.Services
{
    public class NeonTechFileService : IFileService
    {
        private const string containerName = "images";
        private readonly string connectionString = null;

        public NeonTechFileService(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("");
        }

        public Task DeleteProductImage(string path)
        {
            throw new NotImplementedException();
        }

        public Task DeleteProductImageExcept(string[] files)
        {
            throw new NotImplementedException();
        }

        public Task<string> SaveImage(IFormFile file)
        {
            throw new NotImplementedException();
        }
    }
}

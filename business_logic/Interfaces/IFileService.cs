using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace business_logic.Interfaces
{
    public interface IFileService
    {
        Task<string> SaveImage(IFormFile file);
        Task DeleteProductImage(string path);
        Task DeleteProductImageExcept(string[] files);
    }
}

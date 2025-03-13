using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace business_logic.Interfaces
{
    public interface IImageHulk
    {
        Task<string> Save(IFormFile image);
        Task<string> Save(string urlImage);
        bool Delete(string fileName);
    }
}

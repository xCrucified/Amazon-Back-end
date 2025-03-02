using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace business_logic.DTOs
{
    public class CreateProductModel
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int? SubcategoryId { get; set; }
        public string? Description { get; set; }
        public bool? AvailableToPurchase { get; set; }
        [BindProperty(Name = "Images List")]
        public IEnumerable<IFormFile>? Images {  get; set; }
    }
}

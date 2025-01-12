using Microsoft.AspNetCore.Http;
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
        public int? Discount { get; set; }
        public int? CategoryId { get; set; }
        public string? Description { get; set; }
        public bool? AvailableToPurchase { get; set; }
        public IFormFile Image { get; set; }
    }
}

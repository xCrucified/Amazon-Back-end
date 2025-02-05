using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace business_logic.Entities
{
    public class ProductImage
    {
        public int Id { get; set; }
        public string Image {  get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}

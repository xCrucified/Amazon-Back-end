using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace business_logic.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public int Count { get; set; }
        public string DeliveryAddress { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public IEnumerable<ProductDto> Products { get; set; }
    }
}

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
        public string UserId { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
}

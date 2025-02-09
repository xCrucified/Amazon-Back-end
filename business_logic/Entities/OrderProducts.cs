using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace business_logic.Entities
{
    public class OrderProducts
    {
        public int Id { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int ProductId { get; set; }
        public Product Product {  get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int Quantity { get; set; }
    }
}

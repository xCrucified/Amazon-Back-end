using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace business_logic.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime PurchaseDate { get; set; }
        public ICollection<Product> Products { get; set; }
        public float SummaryPrice { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}

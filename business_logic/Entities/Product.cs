using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace business_logic.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? Image {  get; set; }
        public decimal Price { get; set; }
        public bool AvailableToPurchase { get; set; }
        public int Discount { get; set; }
        public int CategoryId { get; set; }
        public ICollection<int> Rates { get; set; }
        //public ICollection<KeyValuePair<string, string>> Properties { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace business_logic.Entities
{
    public class Review
    {
        public int Id { get; set; }
        public string ReviewText { get; set; }
        public int LikesCount { get; set; }
        public int DislikesCount { get; set; }
        public int Rate { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int ProductId {  get; set; }
        public Product Product { get; set; }
    }
}

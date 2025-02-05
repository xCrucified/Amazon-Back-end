using Ardalis.Specification;
using business_logic.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace business_logic.Specifications
{
    public class OrderSpecs
    {
        public class All : Specification<Order>
        {
            public All()
            {
                Query.Include(x => x.User).Include(x => x.Products);
            }
        }

        public class ByUser : Specification<Order>
        {
            public ByUser(string userId)
            {
                Query
                    .Where(x => x.UserId.Equals(userId))
                    .Include(x => x.User)
                    .Include(x => x.Products);
            }
        }

        public class ByDate : Specification<Order> 
        {
            public ByDate(string UserId)
            {
                Query.Where(x => x.UserId == UserId)
                    .Include(x => x.Products)
                    .OrderBy(x => x.PurchaseDate);
            }
        }
    }
}

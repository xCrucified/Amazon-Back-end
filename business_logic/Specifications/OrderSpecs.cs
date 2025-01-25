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
            public All(int id)
            {
                Query.Include(x => x.Id == id);
            }
        }

        public class ByUser : Specification<Order>
        {
            public ByUser(string userId)
            {
                Query
                    .Where(x => x.UserId == userId)
                    .Include(x => x.Products);
            }
        }
    }
}

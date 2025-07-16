using Ardalis.Specification;
using BLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BLL.Specifications
{
    public class OrderSpecs
    {
        public class All : Specification<Order>
        {
            public All()
            {
                Query.Include(x => x.User);
            }
        }

        public class ByUser : Specification<Order>
        {
            public ByUser(string userId)
            {
                Query
                    .Where(x => x.UserId.Equals(userId))
                    .Include(x => x.User);
            }
        }

        public class ByDate : Specification<Order> 
        {
            public ByDate(string UserId)
            {
                Query.Where(x => x.UserId == UserId)
                    .OrderBy(x => x.PurchaseDate);
            }
        }
    }
}

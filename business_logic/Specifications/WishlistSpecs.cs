using Ardalis.Specification;
using business_logic.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace business_logic.Specifications
{
    public class WishlistSpecs
    {
        public class ById : Specification<WishListItem>
        {
            public ById(int id)
            {
                Query.Where(x => x.Id == id);
            }
        }

        public class ByUserId : Specification<WishListItem>
        {
            public ByUserId(string userId)
            {
                Query.Where(x => x.UserId.Equals(userId));
            }
        }

        public class All : Specification<WishListItem>
        {
            public All()
            {
                Query.Include(x => x.Product);
            }
        }

        

    }
}

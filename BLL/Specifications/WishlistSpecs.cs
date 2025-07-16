using Ardalis.Specification;
using BLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Specifications
{
    public class WishlistSpecs
    {
        public class ById : Specification<Wishlist>
        {
            public ById(int id)
            {
                Query.Where(x => x.Id == id);
            }
        }

        public class ByUserId : Specification<Wishlist>
        {
            public ByUserId(string userId)
            {
                Query.Where(x => x.UserId.Equals(userId));
            }
        }

        public class All : Specification<Wishlist>
        {
            public All()
            {
                Query.Include(x => x.Products);
            }
        }

        

    }
}

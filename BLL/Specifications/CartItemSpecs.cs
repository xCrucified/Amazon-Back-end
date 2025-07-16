using Ardalis.Specification;
using BLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Specifications
{
    public class CartItemSpecs
    {
        public class ByUserId : Specification<CartItem>
        {
            public ByUserId(string userId)
            {
                Query.Where(x => x.UserId == userId);
            }
        }

        public class ById : Specification<CartItem>
        {
            public ById(int id)
            {
                Query.Where(x => x.Id == id);
            }
        }

        public class All : Specification<CartItem>
        {
            public All()
            {
                Query.Include(x => x.Product);
            }
        }
    }
}   

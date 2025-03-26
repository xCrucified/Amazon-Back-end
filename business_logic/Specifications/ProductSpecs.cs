using Ardalis.Specification;
using business_logic.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace business_logic.Specifications
{
    public class ProductSpecs
    {
        public class ById : Specification<Product>
        {
            public ById(int id)
            {
                Query.Where(x => x.Id == id);
            }
        }
        public class All : Specification<Product>
        {
            public All()
            {
                Query.Include(x => x.Subcategory).Include(x => x.ProductImages);
            }
        }
        public class ByIds : Specification<Product>
        {
            public ByIds(IEnumerable<int> ids)
            {
                Query.Where(x => ids.Contains(x.Id));
            }
        }
    }
}

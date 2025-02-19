using Ardalis.Specification;
using business_logic.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace business_logic.Specifications
{
    public class ProductPropertiesSpecs
    {
        public class ByProduct : Specification<ProductProperties>
        {
            public ByProduct(int productId, IEnumerable<int> propertyIds)
            {
                Query.Where(x => x.ProductId == productId).Where(x => propertyIds.Contains(x.Id));
            }
        }

        public class AllByProduct : Specification<ProductProperties>
        {
            public AllByProduct(int productId)
            {
                Query.Where(x => x.ProductId == productId);
            }
        }

        public class ById : Specification<ProductProperties>
        {
            public ById(int id)
            {
                Query.Where(x => x.Id == id);
            }
        }

        public class All : Specification<ProductProperties>
        {
            public All()
            {
                Query.Include(x => x.Product);
            }
        }
    }
}

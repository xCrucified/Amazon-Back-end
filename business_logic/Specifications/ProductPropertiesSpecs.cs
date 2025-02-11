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
            public ByProduct(int productId)
            {
                Query.Where(x => x.ProductId == productId);
            }
        }
    }
}

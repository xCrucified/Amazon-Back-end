using Ardalis.Specification;
using business_logic.Entities;

namespace business_logic.Specifications
{
    public class ProductSpecs
    {
        public class ById : Specification<Product>
        {
            public ById(int id)
            {
                Query.Where(x => x.Id == id).Include(x => x.ProductImages).Include(x => x.Subcategory);
            }
        }
        public class All : Specification<Product>
        {
            public All()
            {
                Query.Include(x => x.ProductImages).Include(x => x.Subcategory).Include(x => x.Reviews);
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

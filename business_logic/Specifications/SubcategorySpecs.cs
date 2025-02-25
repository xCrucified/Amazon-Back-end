using Ardalis.Specification;
using business_logic.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace business_logic.Specifications
{
    public class SubcategorySpecs
    {
        public class ById : Specification<Subcategory>
        {
            public ById(int id)
            {
                Query.Where(x => x.Id == id);
            }
        }

        public class ByCategory : Specification<Subcategory>
        {
            public ByCategory(int categoryId)
            {
                Query.Where(x => x.CategoryId == categoryId);
            }
        }

        public class All : Specification<Subcategory>
        {
            public All()
            {
                Query.Include(x => x.Category);
            }
        }

        public class ByIds : Specification<Subcategory>
        {
            public ByIds(IEnumerable<int> ids)
            {
                Query.Where(x => ids.Contains(x.Id));
            }
        }

    }
}

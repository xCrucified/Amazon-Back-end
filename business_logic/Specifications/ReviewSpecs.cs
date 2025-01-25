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
    public class ReviewSpecs
    {
        public class ById : Specification<Review>
        {
            public ById(int id)
            {
                Query.Where(x => x.Id == id);
            }
        }
        public class All : Specification<Review>
        {
            public All(int id)
            {
                Query.Include(x => x.Id == id);
            }
        }
        public class ByIds : Specification<Review>
        {
            public ByIds(IEnumerable<int> ids)
            {
                Query.Where(x => ids.Contains(x.Id));
            }
        }
    }
}

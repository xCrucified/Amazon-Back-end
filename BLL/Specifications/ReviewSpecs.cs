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
            public All()
            {
                Query.Include(x => x.Product).Include(x => x.User);
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

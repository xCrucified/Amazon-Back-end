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
    public class CategorySpecs
    {
        public class ById : Specification<Category>
        {
            public ById(int id)
            {
                Query.Where(x => x.Id == id);
            }
        }
        public class All : Specification<Category>
        {
            public All()
            {
                Query.Include(x => x.Id);
            }
        }
        public class ByIds : Specification<Category>
        {
            public ByIds(IEnumerable<int> ids)
            {
                Query.Where(x => ids.Contains(x.Id));
            }
        }
    }
}

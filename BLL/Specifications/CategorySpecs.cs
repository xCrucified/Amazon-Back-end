using Ardalis.Specification;
using BLL.Entities;

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
                
            }
        }

        public class ByIds : Specification<Category>
        {
            public ByIds(IEnumerable<int> ids)
            {
                Query.Where(x => ids.Contains(x.Id));
                // Якщо у Category є навігаційні властивості, які ти хочеш завантажити
                // наприклад: .Include(x => x.Subcategories)
            }
        }
    }
}
using Ardalis.Specification;
using business_logic.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace business_logic.Specifications
{
    public class RefundSpecs
    {
        public class ByOrder : Specification<Refund>
        {
            public ByOrder(int orderId)
            {
                Query.Where(x => x.OrderId == orderId).Include(x => x.Order);
            }
        }

    }
}

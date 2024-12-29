using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace business_logic.DTOs
{
    internal class CreateRefundModel
    {
        public string Description { get; set; } // here will be reason for refund
        public int OrderId { get; set; }
        public DateTime RefundDate { get; set; }
    }
}

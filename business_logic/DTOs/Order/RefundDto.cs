using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace business_logic.DTOs
{
    public class RefundDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int OrderId { get; set; }
        public DateTime RefundDate { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace business_logic.DTOs
{
    public class CreateProductPropertiesModel
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public int ProductId { get; set; }
    }
}

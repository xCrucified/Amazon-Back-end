using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace business_logic.DTOs
{
    internal class UserToken
    {
        public string RefreshToken { get; set; }
        public string AccessToken { get; set; }
    }
}

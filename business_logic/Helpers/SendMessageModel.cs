using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace business_logic.Helpers
{
    public class SendMessageModel
    {
        public string htmlMessage { get; set; }
        public string Subject { get; set; }
        public string to { get; set; }
    }
}

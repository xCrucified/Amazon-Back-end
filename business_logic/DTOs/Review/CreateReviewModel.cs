using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs
{
    public class CreateReviewModel
    {
        public string UserId { get; set; }
        public int ProductId { get; set; }
        public DateTime PostDate { get; set; }
        public string ReviewText { get; set; }
        public float Rate { get; set; }
    }
}

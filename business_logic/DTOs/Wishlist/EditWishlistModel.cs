using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs.Wishlist
{
    public class EditWishlistModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public bool isPublic { get; set; }
        public IEnumerable<int> Products { get; set; }
    }
}

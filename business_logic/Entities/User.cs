using business_logic.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace business_logic.Entities
{
    public class User : IdentityUser
    {
        public Cart Cart { get; set; }
        public ICollection<Review>? WrittenReviews { get; set; }
        public ICollection<RefreshToken>? RefreshTokens { get; set; }
        public ICollection<Wishlist>? WishLists { get; set; }
    }
}

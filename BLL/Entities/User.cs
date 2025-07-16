using BLL.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Entities
{
    public class User : IdentityUser
    {
        public ICollection<CartItem>? Cart { get; set; }
        public ICollection<Review>? WrittenReviews { get; set; }
        public ICollection<RefreshToken>? RefreshTokens { get; set; }
        public ICollection<Wishlist>? WishLists { get; set; }
    }
}

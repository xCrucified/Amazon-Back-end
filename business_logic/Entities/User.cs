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
        public DateTime? BirthDate { get; set; }
        public ICollection<Product>? WishList { get; set; }
        public ICollection<Product>? Cart { get; set; }
        public ICollection<Review>? WrittenReviews { get; set; }
        public ICollection<RefreshToken>? RefreshTokens { get; set; }
    }
}

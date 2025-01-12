using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace business_logic.Entities
{
    //public enum Plan {Basic, Premium}
    public class User : IdentityUser
    {
        public DateTime? BirthDate { get; set; }
        public string? AvatarPicture {  get; set; }
        public ICollection<Product> WishList { get; set; }
        public ICollection<Product> Cart { get; set; }
        public ICollection<RefreshToken>? RefreshTokens { get; set; }


        //public decimal Balance { get; set; }

    }
}

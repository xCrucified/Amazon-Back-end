using business_logic.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace data_access.Configs
{
    public class WishlistConfig : IEntityTypeConfiguration<Wishlist>
    {
        public void Configure(EntityTypeBuilder<Wishlist> builder)
        {
            builder.HasKey(x => x.Id);
            builder.ToTable("Wishlist");

            builder.HasOne(x => x.User).WithMany(x => x.WishLists).HasForeignKey(x => x.UserId);

            builder.HasOne(x => x.User).WithMany(x => x.WishLists).HasForeignKey(x => x.UserId);

        }
    }
}

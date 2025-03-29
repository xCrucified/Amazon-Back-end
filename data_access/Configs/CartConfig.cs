using business_logic.Entities;
using business_logic.Interfaces;
using business_logic.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace data_access.Configs
{
    public class CartConfig : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.HasKey(x => x.Id);
            builder.ToTable("Carts");
            builder.HasOne(x => x.User).WithMany(x => x.Cart).HasForeignKey(x => x.UserId);
        }
    }
}

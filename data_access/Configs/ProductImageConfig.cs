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
    public class ProductImageConfig : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder.HasKey(x => x.Id);
            builder.ToTable("ProductImages");

            builder.HasOne(x => x.Product).WithMany(x => x.ProductImages).HasForeignKey(x => x.ProductId);
        }
    }
}

using business_logic.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace data_access.data.Database
{
    public static class ModelBuilderExtensions
    {
        public static void SeedData(this ModelBuilder builder)
        {
            builder.Entity<Category>().HasData(new[]
            {
                new Category { Id = 1, Name = "Electronic" },
                new Category { Id = 2, Name = "Furniture" },
                new Category { Id = 3, Name = "Clothing" },
                new Category { Id = 4, Name = "Books" },
                new Category { Id = 5, Name = "Toys" },
                new Category { Id = 6, Name = "Sports" },
                new Category { Id = 7, Name = "Beauty & Health" },
                new Category { Id = 8, Name = "Automotive" },
                new Category { Id = 9, Name = "Groceries" },
                new Category { Id = 10, Name = "Home Appliances" },
                new Category { Id = 11, Name = "Garden & Outdoor" },
            });
        }
    }
}

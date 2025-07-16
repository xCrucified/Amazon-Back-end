using BLL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DAL.data.Database
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

            builder.Entity<Subcategory>().HasData(new[]
            {
                // Electronics
                new Subcategory { Id = 1, Name = "Phones", CategoryId = 1 },
                new Subcategory { Id = 2, Name = "Laptops", CategoryId = 1 },
                new Subcategory { Id = 3, Name = "Televisions", CategoryId = 1 },

                // Furniture
                new Subcategory { Id = 4, Name = "Sofas", CategoryId = 2 },
                new Subcategory { Id = 5, Name = "Beds", CategoryId = 2 },
                new Subcategory { Id = 6, Name = "Dining Tables", CategoryId = 2 },

                // Clothing
                new Subcategory { Id = 7, Name = "Men's Clothing", CategoryId = 3 },
                new Subcategory { Id = 8, Name = "Women's Clothing", CategoryId = 3 },
                new Subcategory { Id = 9, Name = "Kids' Clothing", CategoryId = 3 },

                // Books
                new Subcategory { Id = 10, Name = "Fiction", CategoryId = 4 },
                new Subcategory { Id = 11, Name = "Non-Fiction", CategoryId = 4 },
                new Subcategory { Id = 12, Name = "Educational", CategoryId = 4 },

                // Toys
                new Subcategory { Id = 13, Name = "Action Figures", CategoryId = 5 },
                new Subcategory { Id = 14, Name = "Board Games", CategoryId = 5 },
                new Subcategory { Id = 15, Name = "Dolls", CategoryId = 5 },

                // Sports
                new Subcategory { Id = 16, Name = "Fitness Equipment", CategoryId = 6 },
                new Subcategory { Id = 17, Name = "Outdoor Sports", CategoryId = 6 },
                new Subcategory { Id = 18, Name = "Team Sports", CategoryId = 6 },

                // Beauty & Health
                new Subcategory { Id = 19, Name = "Skincare", CategoryId = 7 },
                new Subcategory { Id = 20, Name = "Hair Care", CategoryId = 7 },
                new Subcategory { Id = 21, Name = "Makeup", CategoryId = 7 },

                // Automotive
                new Subcategory { Id = 22, Name = "Car Accessories", CategoryId = 8 },
                new Subcategory { Id = 23, Name = "Motorcycle Parts", CategoryId = 8 },
                new Subcategory { Id = 24, Name = "Tires & Wheels", CategoryId = 8 },

                // Groceries
                new Subcategory { Id = 25, Name = "Fruits & Vegetables", CategoryId = 9 },
                new Subcategory { Id = 26, Name = "Beverages", CategoryId = 9 },
                new Subcategory { Id = 27, Name = "Snacks", CategoryId = 9 },

                // Home Appliances
                new Subcategory { Id = 28, Name = "Refrigerators", CategoryId = 10 },
                new Subcategory { Id = 29, Name = "Washing Machines", CategoryId = 10 },
                new Subcategory { Id = 30, Name = "Microwaves", CategoryId = 10 },

                // Garden & Outdoor
                new Subcategory { Id = 31, Name = "Gardening Tools", CategoryId = 11 },
                new Subcategory { Id = 32, Name = "Outdoor Furniture", CategoryId = 11 },
                new Subcategory { Id = 33, Name = "BBQ & Grills", CategoryId = 11 }
            });

            //builder.Entity<Product>().HasData(new[] {
            //    new Product {Id = 1, Name = ""}            
            //});
        }
    }
}

using business_logic.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace data_access.data.configs
{
    public static class ModelBuilderExtensions
    {

        public static void SeedData(this ModelBuilder builder)
        {
            builder.Entity<User>().HasData(new User() { Id="1", UserName="dota2player" });
        }
    }
}

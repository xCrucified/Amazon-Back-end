using AutoMapper;
using business_logic.Entities;
using business_logic.Interfaces;
using Microsoft.AspNetCore.Cors.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace business_logic.Services
{
    public class OrderService
    {
        private readonly IMapper mapper;
        private readonly IRepository<Order> orderR;
        private readonly IRepository<Product> productR; 
        private readonly ICartService cartService;
    }
}

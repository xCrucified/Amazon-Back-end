using AutoMapper;
using business_logic.DTOs;
using business_logic.Entities;
using business_logic.Interfaces;
using business_logic.Specifications;
using Microsoft.AspNetCore.Cors.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace business_logic.Services
{
    public class OrderService : IOrderService
    {
        private readonly IMapper mapper;
        private readonly IRepository<Order> orderR;
        private readonly IRepository<Product> productR; 
        private readonly ICartService cartService;


        public OrderService(IMapper mapper,
                            IRepository<Order> orderR,
                            IRepository<Product> productR,
                            ICartService cartService)
        {
            this.mapper = mapper;
            this.orderR = orderR;
            this.productR = productR;
            this.cartService = cartService;
        }

        public async Task Create(string userId)
        {
            var ids = cartService.GetProductIds();
            var products = await productR.GetListBySpec(new ProductSpecs.ByIds(ids));

            if (!products.ToList().Select(x => x.AvailableToPurchase).Contains(false))
            {
                var order = new Order()
                {
                    PurchaseDate = DateTime.Now,
                    UserId = userId,
                    Products = products.ToList()
                };

                orderR.Insert(order);
                orderR.Save();
            }
            else throw new HttpException("Some of products are not available for purchase now", HttpStatusCode.BadRequest);
        }
        public async Task<IEnumerable<OrderDto>> GetAllByUser(string userId)
        {
            var items = await orderR.GetListBySpec(new OrderSpecs.ByUser(userId));
            return mapper.Map<IEnumerable<OrderDto>>(items);
        }
    }
}

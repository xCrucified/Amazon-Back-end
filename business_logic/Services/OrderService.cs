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
            var productIds = cartService.GetProductIds();

            if (productIds == null || !productIds.Any())
            {
                throw new Exception("Cart is empty.");
            }

            var products = await productR.GetListBySpec(new ProductSpecs.ByIds(productIds));

            if (products.Any(p => !p.AvailableToPurchase))
            {
                throw new HttpException("Some of the products are not available for purchase now", HttpStatusCode.BadRequest);
            }


            var order = new Order
            {
                UserId = userId,
                PurchaseDate = DateTime.Now,
                TotalPrice = products.Sum(p => p.Price),
                OrderProducts = new List<OrderProduct>() 
            };

            foreach (var product in products)
            {
                order.OrderProducts.Add(new OrderProduct
                {
                    ProductId = product.Id,
                    Quantity = 1
                });
            }

            orderR.Insert(order);
            orderR.Save();
        }
        public async Task<IEnumerable<OrderDto>> GetAllByUser(string userId)
        {
            var items = await orderR.GetListBySpec(new OrderSpecs.ByUser(userId));
            return mapper.Map<IEnumerable<OrderDto>>(items);
        }
    }
}

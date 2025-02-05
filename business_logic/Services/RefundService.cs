using AutoMapper;
using business_logic.DTOs;
using business_logic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace business_logic.Services
{
    public class RefundService : IRefundService
    {
        private readonly IRepository<RefundDto> refundR;
        private readonly Mapper mapper;

        public async Task Create(RefundDto refund)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<RefundDto>> GetAllByUser(string userId)
        {
            throw new NotImplementedException();
        }
    }
}

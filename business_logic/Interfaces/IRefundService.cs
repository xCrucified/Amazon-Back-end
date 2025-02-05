using business_logic.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace business_logic.Interfaces
{
    public interface IRefundService
    {
        Task<IEnumerable<RefundDto>> GetAllByUser(string userId);
        Task Create(RefundDto dto);
    }
}

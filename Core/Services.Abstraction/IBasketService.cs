using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesAbstractions
{
    public interface IBasketService
    {
        Task<BasketDto?> GetBasketAsync(string Id);
        Task<BasketDto?> UpdateBasketAsync(BasketDto basketDto);
        Task<bool> DeleteBasketAsync(string Id);
    }
}
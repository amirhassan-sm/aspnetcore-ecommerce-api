using Application.DTOs.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contrast.QueryServices
{
    public interface IBasketQueryService
    {
        Task<BasketResultDto> GetBasketAsync(List<BasketItemDto> basketItems);
    }
}

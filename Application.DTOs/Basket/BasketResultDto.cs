using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Basket
{
    public class BasketResultDto
    {
        public BasketSummeryDto Summery { get; set; } = new();

        public List<BasketListItemDto> basketListItems { get; set; } = new();
    }
}

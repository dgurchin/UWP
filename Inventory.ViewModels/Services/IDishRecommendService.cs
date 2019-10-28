using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Inventory.Models;

namespace Inventory.Services
{
    public interface IDishRecommendService
    {
        Task<IList<DishRecommendModel>> GetDishRecommendsAsync(Guid dishGuid);
        Task<IList<OrderRecommendModel>> GetOrderRecommendsAsync(Guid dishGuid);
    }
}
